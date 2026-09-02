using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Common;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Models;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.Services;

namespace SportsTracker.App.Controllers
{
    [ApiController]
    [Route("dev")]
    public sealed class DevController(IScoreboardService scoreboardService, ILeagueLeadersService leagueLeadersService, IAthleteService athleteService) : ControllerBase
    {
        [HttpGet("scoreboard/{league}")]
        public async Task<ActionResult<CachedScoreboard>> Scoreboard(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return scoreboard is null ? NotFound() : Ok(scoreboard);
        }

        [HttpGet("scoreboard/{league}/{date}")]
        public async Task<ActionResult<CachedScoreboard>> Scoreboard(League league, DateOnly date, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, date, cancellationToken);
            
            return scoreboard is null ? NotFound() : Ok(scoreboard);
        }

        [HttpGet("league-leaders/{league}")]
        public async Task<ActionResult<LeagueLeaders>> LeagueLeaders(League league, CancellationToken cancellationToken)
        {
            LeagueLeaders? leagueStatistics = await leagueLeadersService.GetLeadersAsync(league, cancellationToken);
            
            return leagueStatistics is null ? NotFound() : Ok(leagueStatistics);
        }

        [HttpGet("athlete/{league}/{athleteId}")]
        public async Task<ActionResult<AthleteDetails>> AthleteDetails(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteDetails? athleteDetails = await athleteService.GetAthleteDetailsAsync(league, athleteId, cancellationToken);
            
            return athleteDetails is null ? NotFound() : Ok(athleteDetails);
        }

        [HttpGet("athlete-overview/{league}/{athleteId}")]
        public async Task<ActionResult<AthleteOverview>> AthleteOverview(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteOverview? athleteOverview = await athleteService.GetAthleteOverviewAsync(league, athleteId, cancellationToken);
            
            return athleteOverview is null ? NotFound() : Ok(athleteOverview);
        }

        [HttpGet("athlete-splits/{league}/{athleteId}")]
        public async Task<ActionResult<AthleteSplits>> AthleteSplits(League league, string athleteId, CancellationToken cancellationToken)
        {
            AthleteSplits? athleteSplits = await athleteService.GetAthleteSplitsAsync(league, athleteId, cancellationToken);
            
            return athleteSplits is null ? NotFound() : Ok(athleteSplits);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string q, [FromServices] IEspnApiClient espnApiClient, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest();
            }
            
            string endpoint = EspnEndpoints.Search(q, 25);

            ApiResult<JsonElement> result = await espnApiClient.GetAsync<JsonElement>(endpoint, cancellationToken);
            
            return result.Success ? Ok(result.Value) : StatusCode(result.StatusCode ?? 500, result.Error);
        }
    }
}