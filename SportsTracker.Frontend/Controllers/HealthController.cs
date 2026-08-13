using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Services.Api;

namespace SportsTracker.Frontend.Controllers
{
    [Route("health")]
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Health([FromServices] SportsApiClient api, CancellationToken cancellationToken)
        {
            bool healthy = await api.IsHealthyAsync(cancellationToken);

            return Json(new
            {
                healthy
            });
        }
    }
}