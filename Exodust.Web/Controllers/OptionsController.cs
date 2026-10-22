using Exodust.Web.Data;
using Exodust.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Controllers;

[Authorize]
public class OptionsController(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var options = await GetOrCreateOptionsAsync();
        return View(options);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(bool isPaid)
    {
        var options = await GetOrCreateOptionsAsync();
        options.IsPaid = isPaid;
        await dbContext.SaveChangesAsync();
        TempData["StatusMessage"] = isPaid ? "Paid plan enabled." : "Free plan enabled.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<PlayerOptions> GetOrCreateOptionsAsync()
    {
        var user = await userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User is not authenticated.");
        var options = await dbContext.PlayerOptions.SingleOrDefaultAsync(x => x.UserId == user.Id);
        if (options is not null)
        {
            return options;
        }

        options = new PlayerOptions { UserId = user.Id };
        dbContext.PlayerOptions.Add(options);
        await dbContext.SaveChangesAsync();
        return options;
    }
}
