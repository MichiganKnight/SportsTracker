using Microsoft.AspNetCore.Mvc;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.Controllers
{
    public class LeagueController : Controller
    {
        public async Task<IActionResult> Index(League league)
        {
            return View();
        }
    }
}