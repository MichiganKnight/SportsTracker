using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/standings")]
    public sealed class StandingsController(IStandingsService standingsService, IGroupsService groupsService, IStandingsGroupingService groupingService) : ApiControllerBase
    {
        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<LeagueStandings>>> GetStandings(League league, CancellationToken cancellationToken)
        {
            Task<LeagueStandings?> standingsTask = standingsService.GetStandingsAsync(league, cancellationToken);
            Task<IReadOnlyList<SportsGroup>?> groupsTask = groupsService.GetGroupsAsync(league, cancellationToken);
            
            await Task.WhenAll(standingsTask, groupsTask);
            
            LeagueStandings? standings = await standingsTask;
            IReadOnlyList<SportsGroup>? groups = await groupsTask;

            if (standings is null)
            {
                return NotFound();
            }

            if (groups is not null)
            {
                standings = groupingService.AddDivisionGroups(standings, groups);
            }
            
            return ApiOk(standings);
        }
    }
}