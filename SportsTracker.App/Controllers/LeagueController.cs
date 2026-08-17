using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    [Route("league")]
    public sealed class LeagueController(IScoreboardService scoreboardService, ILeagueViewModelMapper leagueViewModelMapper) : Controller
    {
        [HttpGet("{league}")]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            LeaguePageViewModel? viewModel = await GetLeaguePageViewModelAsync(league, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return View(viewModel);
        }

        [HttpGet("GameSections")]
        public async Task<IActionResult> GameSections(League league, CancellationToken cancellationToken)
        {
            LeaguePageViewModel? viewModel = await GetLeaguePageViewModelAsync(league, cancellationToken);
            
            if (viewModel is null)
            {
                return NotFound();
            }
            
            return PartialView("Partials/_LeagueGameSections", viewModel);
        }

        private async Task<LeaguePageViewModel?> GetLeaguePageViewModelAsync(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return leagueViewModelMapper.Map(scoreboard);
        }
    }
}