using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Models;
using SportsTracker.App.Services;

namespace SportsTracker.App.Controllers
{
    [ApiController]
    [Route("dev")]
    public sealed class DevController(IScoreboardService scoreboardService) : ControllerBase
    {
        [HttpGet("scoreboard/{league}")]
        public async Task<ActionResult<CachedScoreboard>> Scoreboard(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return scoreboard is null ? NotFound() : Ok(scoreboard);
        }
    }
}