using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/standings")]
    public sealed class StandingsController : Controller
    {
        private readonly IStandingsService _standingsService;
        
        public StandingsController(IStandingsService standingsService)
        {
            _standingsService = standingsService;
        }

        [HttpGet("{league}")]
        public async Task<ActionResult<ApiResponse<LeagueStandings>>> GetStandings(League league, CancellationToken cancellationToken)
        {
            LeagueStandings? standings = await _standingsService.GetStandingsAsync(league, cancellationToken);

            if (standings is null)
            {
                return NotFound();
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