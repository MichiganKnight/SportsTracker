using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/games")]
    public sealed class PlayByPlayController(IPlayByPlayService playByPlayService) : ApiControllerBase
    {
        [HttpGet("{league}/{gameId}/playbyplay")]
        public async Task<ActionResult<ApiResponse<GamePlayByPlay>>> GetPlayByPlay(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePlayByPlay? playByPlay = await playByPlayService.GetPlayByPlayAsync(league, gameId, cancellationToken);

            return playByPlay is null ? NotFound() : ApiOk(playByPlay);
        }
    }
}