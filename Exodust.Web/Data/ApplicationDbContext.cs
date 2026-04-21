using Exodust.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<PlayerOptions> PlayerOptions => Set<PlayerOptions>();
    public DbSet<UserProgress> UserProgress => Set<UserProgress>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PlayerOptions>(entity =>
        {
            entity.ToTable("UserOptions");
            entity.HasIndex(x => x.UserId).IsUnique();
        });

        builder.Entity<UserProgress>(entity =>
        {
            entity.ToTable("UserProgress");
            entity.HasIndex(x => x.UserId).IsUnique();
        });
    }
}
