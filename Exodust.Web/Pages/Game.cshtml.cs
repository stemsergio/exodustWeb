using Exodust.Web.Data;
using Exodust.Web.Hubs;
using Exodust.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Pages;

[Authorize]
public class GameModel(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager,
    IHubContext<GameHub> hubContext) : PageModel
{
    public int Level { get; private set; }
    public int ExperiencePoints { get; private set; }

    public async Task OnGetAsync()
    {
        var progress = await GetOrCreateProgressAsync();
        Level = progress.Level;
        ExperiencePoints = progress.ExperiencePoints;
    }

    public async Task<IActionResult> OnPostGainXpAsync()
    {
        var progress = await GetOrCreateProgressAsync();
        progress.ExperiencePoints += 10;
        progress.Level = (progress.ExperiencePoints / 100) + 1;
        progress.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        await hubContext.Clients.User(progress.UserId).SendAsync("ProgressUpdated", progress.Level, progress.ExperiencePoints);
        return RedirectToPage();
    }

    private async Task<UserProgress> GetOrCreateProgressAsync()
    {
        var user = await userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User is not authenticated.");
        var progress = await dbContext.UserProgress.SingleOrDefaultAsync(x => x.UserId == user.Id);
        if (progress is not null)
        {
            return progress;
        }

        progress = new UserProgress { UserId = user.Id };
        dbContext.UserProgress.Add(progress);
        await dbContext.SaveChangesAsync();
        return progress;
    }
}
