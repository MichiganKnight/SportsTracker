using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.Standings;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Controllers
{
    public sealed class StandingsController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly IStandingsMapper _mapper;
        
        public StandingsController(ISportsApiClient api, IStandingsMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(League league, StandingsView view, CancellationToken cancellationToken = default)
        {
            ApiResponse<LeagueStandings>? response = await _api.GetStandingsAsync(league, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            StandingsViewModel viewModel = _mapper.Map(response.Data, view, response.TimestampUtc);
            
            return View(viewModel);
        }
    }
}