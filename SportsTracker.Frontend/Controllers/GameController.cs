using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.GameDetails;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Frontend.Controllers
{
    [Route("game")]
    public sealed class GameController : Controller
    {
        private readonly ISportsApiClient _api;
        private readonly IGameDetailsMapper _mapper;
        
        public GameController(ISportsApiClient api, IGameDetailsMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }

        [HttpGet("{league}/{gameId}")]
        public async Task<IActionResult> Index(League league, string gameId, CancellationToken cancellationToken)
        {
            ApiResponse<GameDetails>? response = await _api.GetGameDetailsAsync(league, gameId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            GameDetailsViewModel viewModel = _mapper.Map(response.Data);
            
            return View(viewModel);
        }
    }
}