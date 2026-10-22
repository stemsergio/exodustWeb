using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Data;

// On startup: applies pending migrations, then seeds roles and the super admin.
// Migrations themselves are created by hand (dotnet ef migrations add).
public class DbInitializer(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration,
    ILogger<DbInitializer> logger)
{
    public async Task InitializeAsync()
    {
        if (!dbContext.Database.GetMigrations().Any())
        {
            logger.LogWarning("No migrations exist yet. Create one with 'dotnet ef migrations add'. Seeding skipped.");
            return;
        }

        // Same as 'dotnet ef database update'. Creates the database if it does not exist.
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            logger.LogInformation("Applying pending migrations.");
            await dbContext.Database.MigrateAsync();
        }

        foreach (var role in new[] { Roles.SuperAdmin, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Kept in user-secrets / environment variables, never in the repo.
        var email = configuration["Seed:SuperAdminEmail"];
        var password = configuration["Seed:SuperAdminPassword"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Seed:SuperAdminEmail or Seed:SuperAdminPassword is not configured. Super admin not created.");
            return;
        }

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                logger.LogError("Could not create the super admin: {Errors}",
                    string.Join("; ", result.Errors.Select(e => e.Description)));
                return;
            }
        }

        foreach (var role in new[] { Roles.SuperAdmin, Roles.User })
        {
            if (!await userManager.IsInRoleAsync(admin, role))
            {
                await userManager.AddToRoleAsync(admin, role);
            }
        }
    }
}
