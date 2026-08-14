using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/teams")]
    public sealed class TeamController(ITeamDetailsService teamDetailsService, ITeamScheduleService teamScheduleService, ITeamRosterService teamRosterService) : ApiControllerBase
    {
        [HttpGet("{league}/{teamId}")]
        public async Task<ActionResult<ApiResponse<TeamDetails>>> GetTeam(League league, string teamId, CancellationToken cancellationToken)
        {
            TeamDetails? team = await teamDetailsService.GetTeamDetailsAsync(league, teamId, cancellationToken);
            
            return team is null ? NotFound() : ApiOk(team);
        }

        [HttpGet("{league}/{teamId}/schedule")]
        public async Task<ActionResult<ApiResponse<TeamSchedule>>> GetSchedule(League league, string teamId, CancellationToken cancellationToken)
        {
            TeamSchedule? schedule = await teamScheduleService.GetTeamScheduleAsync(league, teamId, cancellationToken);
            
            return schedule is null ? NotFound() : ApiOk(schedule);
        }

        [HttpGet("{league}/{teamId}/roster")]
        public async Task<ActionResult<ApiResponse<TeamRoster>>> GetRoster(League league, string teamId, CancellationToken cancellationToken)
        {
            TeamRoster? roster = await teamRosterService.GetTeamRosterAsync(league, teamId, cancellationToken);
            
            return roster is null ? NotFound() : ApiOk(roster);
        }
    }
}