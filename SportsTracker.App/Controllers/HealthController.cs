using Microsoft.AspNetCore.Mvc;

namespace SportsTracker.App.Controllers
{
    [Route("health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                healthy = true
            });
        }
    }
}