using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    public sealed class GameDetailsController : Controller
    {
        private readonly IGameDetailsService _gameDetailsService;
        
        public GameDetailsController(IGameDetailsService gameDetailsService)
        {
            _gameDetailsService = gameDetailsService;
        }

        [HttpGet("{league}/{gameId}")]
        public async Task<ActionResult<ApiResponse<GameDetails>>> GetGame(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetails? details = await _gameDetailsService.GetGameDetailsAsync(league, gameId, cancellationToken);

            if (details is null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse<GameDetails>
            {
                Data = details,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}