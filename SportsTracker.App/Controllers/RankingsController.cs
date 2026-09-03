using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.Rankings;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.Rankings;

namespace SportsTracker.App.Controllers
{
    [Route("league/{league}/rankings")]
    public sealed class RankingsController(IRankingsService rankingsService, IRankingsViewModelMapper viewModelMapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            if (league != League.CFB)
            {
                return NotFound();
            }

            LeagueRankings? rankings = await rankingsService.GetRankingsAsync(league, cancellationToken);

            if (rankings is null)
            {
                return NotFound();
            }
            
            RankingsViewModel viewModel = viewModelMapper.Map(league, rankings);
            
            return View(viewModel);
        }
    }
}