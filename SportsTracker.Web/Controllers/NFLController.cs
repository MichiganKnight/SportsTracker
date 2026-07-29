using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class NFLController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}