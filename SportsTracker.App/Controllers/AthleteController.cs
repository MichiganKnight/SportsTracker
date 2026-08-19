using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Controllers
{
    [Route("athlete")]
    public sealed class AthleteController(IAthleteDetailsService athleteDetailsService, IAthleteDetailsViewModelMapper athleteDetailsViewModelMapper) : Controller
    {
        [HttpGet("{league}/{athleteId}")]
        public async Task<IActionResult> Index(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetails? athlete = await athleteDetailsService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            
            if (athlete is null)
            {
                return NotFound();
            }
            
            AthleteDetailsViewModel viewModel = athleteDetailsViewModelMapper.Map(athlete);
            
            return View(viewModel);
        }
    }
}