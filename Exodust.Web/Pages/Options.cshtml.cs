using Exodust.Web.Data;
using Exodust.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Pages;

[Authorize]
public class OptionsModel(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty]
    public bool IsPaid { get; set; }

    public async Task OnGetAsync()
    {
        var options = await GetOrCreateOptionsAsync();
        IsPaid = options.IsPaid;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var options = await GetOrCreateOptionsAsync();
        options.IsPaid = IsPaid;
        await dbContext.SaveChangesAsync();
        TempData["StatusMessage"] = IsPaid ? "Paid plan enabled." : "Free plan enabled.";
        return RedirectToPage();
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
