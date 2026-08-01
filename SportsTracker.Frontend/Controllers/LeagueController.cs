using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.Pages;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Controllers
{
    [Route("league")]
    public sealed class LeagueController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly ILeagueMapper _mapper;
        
        public LeagueController(ISportsApiClient api, ILeagueMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }
        
        [HttpGet("{league}")]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            ApiResponse<CachedScoreboard>? response = await _api.GetLeagueAsync(league, cancellationToken);

            if (response is null)
            {
                return NotFound();
            }

            LeaguePageViewModel model = _mapper.Map(response.Data);
            
            return View(model);
        }
    }
}

/*
 * Sounds really good. I'm ready. Also, further down the line, I assume we'll add more to this page for standings, stats, past games, etc...? Anyways, let's add this.
*/