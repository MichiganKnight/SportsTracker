using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
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