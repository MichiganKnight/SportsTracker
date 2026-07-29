using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class CFBController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}