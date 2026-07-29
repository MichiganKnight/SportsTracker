using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class NHLController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}