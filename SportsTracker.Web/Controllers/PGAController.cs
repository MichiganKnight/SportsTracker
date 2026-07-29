using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class PGAController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}