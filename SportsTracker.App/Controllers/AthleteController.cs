using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.Services;

namespace SportsTracker.App.Controllers
{
    [Route("athlete")]
    public sealed class AthleteController(IAthleteDetailsService athleteDetailsService) : Controller
    {
        [HttpGet("{league}/{athleteId}")]
        public async Task<IActionResult> Index(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetails? athlete = await athleteDetailsService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            
            if (athlete is null)
            {
                return NotFound();
            }
            
            return View(athlete);
        }
    }
}