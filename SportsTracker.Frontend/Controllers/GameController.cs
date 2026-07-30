using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Frontend.Controllers
{
    public class GameController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}