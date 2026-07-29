using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class NBAController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}