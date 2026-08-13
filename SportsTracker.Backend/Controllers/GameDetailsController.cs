using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/games")]
    public sealed class GameDetailsController(IGameDetailsService gameDetailsService) : ApiControllerBase
    {
        [HttpGet("{league}/{gameId}")]
        public async Task<ActionResult<ApiResponse<GameDetails>>> GetGame(League league, string gameId, CancellationToken cancellationToken)
        {
            GameDetails? details = await gameDetailsService.GetGameDetailsAsync(league, gameId, cancellationToken);

            return details is null ? NotFound() : ApiOk(details);
        }
    }
}