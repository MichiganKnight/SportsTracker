using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/scoreboard")]
    public class ScoreboardController(IScoreboardService scoreboardService) : ControllerBase
    {
        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<CachedScoreboard>>> GetScoreboard(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);

            if (scoreboard is null)
            {
                return NotFound();
            }
            
            return Ok(new ApiResponse<CachedScoreboard>
            {
                Data = scoreboard,
                TimestampUtc = scoreboard.LastUpdatedUtc,
                Version = "v1"
            });
        }
    }
}