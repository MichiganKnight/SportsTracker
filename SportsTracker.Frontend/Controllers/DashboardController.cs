using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly IDashboardMapper _mapper;
        
        public DashboardController(ISportsApiClient api, IDashboardMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            Dictionary<League, IReadOnlyList<Game>> scoreboards = new();

            foreach (League league in LeagueConfiguration.All)
            {
                try
                {
                    ApiResponse<IReadOnlyList<Game>>? response = await _api.GetScoreboardAsync(league, cancellationToken);

                    scoreboards[league] = response?.Data ?? [];
                }
                catch
                {
                    scoreboards[league] = [];
                }
            }
            
            DashboardViewModel model = _mapper.Map(scoreboards);

            return View(model);
        }
    }
}