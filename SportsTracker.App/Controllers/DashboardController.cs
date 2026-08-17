using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.DashboardInfo;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    public class DashboardController(IScoreboardService scoreboardService, IDashboardViewModelMapper dashboardViewModelMapper) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            Dictionary<League, IReadOnlyList<Game>> scoreboards = new();

            foreach (League league in LeagueConfiguration.All)
            {
                CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
                    
                scoreboards[league] = scoreboard?.Games ?? [];
            }
            
            DashboardViewModel model = dashboardViewModelMapper.Map(scoreboards);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LeagueSection(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);

            LeagueSectionViewModel viewModel = dashboardViewModelMapper.MapLeague(league, scoreboard?.Games);
            
            return PartialView("Dashboard/_LeagueSection", viewModel);
        }
    }
}