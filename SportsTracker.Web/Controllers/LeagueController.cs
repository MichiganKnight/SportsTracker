using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
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