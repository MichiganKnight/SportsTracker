using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Groups;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/standings")]
    public sealed class StandingsController : Controller
    {
        private readonly IStandingsService _standingsService;
        private readonly IGroupsService _groupsService;
        private readonly IStandingsGroupingService _groupingService;

        public StandingsController(IStandingsService standingsService, IGroupsService groupsService, IStandingsGroupingService groupingService)
        {
            _standingsService = standingsService;
            _groupsService = groupsService;
            _groupingService = groupingService;
        }

        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<LeagueStandings>>> GetStandings(League league, CancellationToken cancellationToken)
        {
            LeagueStandings? standings = await _standingsService.GetStandingsAsync(league, cancellationToken);

            if (standings is null)
            {
                return NotFound();
            }
            
            IReadOnlyList<SportsGroup>? groups = await _groupsService.GetGroupsAsync(league, cancellationToken);

            if (groups is not null)
            {
                standings = _groupingService.AddDivisionGroups(standings, groups);
            }
            
            return Ok(new ApiResponse<LeagueStandings>
            {
                Data = standings,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}