using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Models;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.Services;

namespace SportsTracker.App.Controllers
{
    [ApiController]
    [Route("dev")]
    public sealed class DevController(IScoreboardService scoreboardService, IAthleteDetailsService athleteDetailsService) : ControllerBase
    {
        [HttpGet("scoreboard/{league}")]
        public async Task<ActionResult<CachedScoreboard>> Scoreboard(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return scoreboard is null ? NotFound() : Ok(scoreboard);
        }

        [HttpGet("athlete/{league}/{athleteId}")]
        public async Task<ActionResult<AthleteDetails>> AthleteDetails(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetails? athleteDetails = await athleteDetailsService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            
            return athleteDetails is null ? NotFound() : Ok(athleteDetails);
        }
    }
}