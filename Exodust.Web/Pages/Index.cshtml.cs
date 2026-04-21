using Exodust.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exodust.Web.Pages;

public class IndexModel(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager) : PageModel
{
    public bool ShowAdsBanner { get; private set; }

    public async Task OnGetAsync()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return;
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return;
        }

        var options = await dbContext.PlayerOptions.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == user.Id);
        ShowAdsBanner = options is null || !options.IsPaid;
    }
}
