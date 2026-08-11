using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.LeagueInfo;
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
            
            return PartialView("Game/_LeagueGameSections", viewModel);
        }

        private async Task<LeaguePageViewModel?> GetLeaguePageViewModelAsync(League league, CancellationToken cancellationToken)
        {
            ApiResponse<CachedScoreboard>? response = await _api.GetLeagueAsync(league, cancellationToken);
            
            return _mapper.Map(response?.Data);
        }
    }
}