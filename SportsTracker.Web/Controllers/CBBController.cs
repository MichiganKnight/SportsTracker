using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Web.Controllers
{
    public class CBBController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}