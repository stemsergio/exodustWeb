using System.Diagnostics;
using Exodust.Web.Data;
using Exodust.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exodust.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Logged-in players go straight to the game; everyone else sees the landing page.
        if (User.IsInRole(Roles.User))
        {
            return RedirectToAction("Index", "Game");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
