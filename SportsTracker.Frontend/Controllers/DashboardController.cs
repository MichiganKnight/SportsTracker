using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.DashboardInfo;
using SportsTracker.Frontend.ViewModels.LeagueInfo;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Frontend.Controllers
{
    public class DashboardController(ISportsApiClient api, IDashboardMapper mapper) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            Dictionary<League, IReadOnlyList<Game>> scoreboards = new();

            foreach (League league in LeagueConfiguration.All)
            {
                try
                {
                    ApiResponse<IReadOnlyList<Game>>? response = await api.GetScoreboardAsync(league, cancellationToken);

                    if (response is not null)
                    {
                        scoreboards[league] = response?.Data ?? [];
                    }
                }
                catch
                {
                    scoreboards[league] = [];
                }
            }
            
            DashboardViewModel model = mapper.Map(scoreboards);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LeagueSection(League league, CancellationToken cancellationToken)
        {
            ApiResponse<CachedScoreboard>? response = await api.GetLeagueAsync(league, cancellationToken);

            LeagueSectionViewModel viewModel = mapper.MapLeague(league, response?.Data?.Games);
            
            return PartialView("_LeagueSection", viewModel);
        }
    }
}