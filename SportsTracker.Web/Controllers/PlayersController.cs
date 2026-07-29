using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class PlayersController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}