using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Backend.Integrations.ESPN.Endpoints;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/espn")]
    public class EspnController : ControllerBase
    {
        private readonly IEspnApiClient _espnApiClient;

        public EspnController(IEspnApiClient espnApiClient)
        {
            _espnApiClient = espnApiClient;
        }

        [HttpGet("scoreboard/nfl")]
        public async Task<IActionResult> GetNflScoreboard(CancellationToken cancellationToken)
        {
            string endpoint = EspnEndpoints.Scoreboard(League.NFL);

            ScoreboardResponseDto? result = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);
            
            return Ok(result);
        }
    }
}