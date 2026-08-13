using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/scoreboard")]
    public class ScoreboardController(IScoreboardService scoreboardService) : ApiControllerBase
    {
        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<CachedScoreboard>>> GetScoreboard(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);

            return scoreboard is null ? NotFound() : ApiOk(scoreboard, scoreboard.LastUpdatedUtc);
        }
    }
}