using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.Frontend.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            
            return View();
        }
    }
}