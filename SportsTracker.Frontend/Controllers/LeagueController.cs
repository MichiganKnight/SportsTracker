using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Frontend.Controllers
{
    public class LeagueController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}