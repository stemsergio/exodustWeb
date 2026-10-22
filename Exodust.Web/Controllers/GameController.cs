using Exodust.Web.Data;
using Exodust.Web.Hubs;
using Exodust.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Controllers;

[Authorize(Roles = Roles.User)]
public class GameController(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager,
    IHubContext<NotificationsHub> hubContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        var progress = await GetOrCreateProgressAsync();
        var options = await dbContext.PlayerOptions.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == progress.UserId);

        return View(new GameViewModel
        {
            Level = progress.Level,
            ExperiencePoints = progress.ExperiencePoints,
            ShowAdsBanner = options is null || !options.IsPaid,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GainXp()
    {
        var progress = await GetOrCreateProgressAsync();
        progress.ExperiencePoints += 10;
        progress.Level = (progress.ExperiencePoints / 100) + 1;
        progress.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        await hubContext.Clients.User(progress.UserId).SendAsync("ProgressUpdated", progress.Level, progress.ExperiencePoints);
        return RedirectToAction(nameof(Index));
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
