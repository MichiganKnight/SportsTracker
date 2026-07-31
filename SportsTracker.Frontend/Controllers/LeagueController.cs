using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.Shared;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly ILeagueMapper _mapper;
        
        public LeagueController(ISportsApiClient api, ILeagueMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }
        
        [HttpGet("/league/{league}")]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            ApiResponse<CachedScoreboard>? response = await _api.GetScoreboardAsync(league, cancellationToken);

            if (response is null)
            {
                return NotFound();
            }
            
            LeaguePageViewModel model = _mapper.Map(response.Data);
            
            return View(model);
        }
    }
}