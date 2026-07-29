using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Integrations.ESPN.DTOs;
using SportsTracker.Shared.Enums;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IEspnApiClient _espnApiClient;

        public TestController(IEspnApiClient espnApiClient)
        {
            _espnApiClient = espnApiClient;
        }

        [HttpGet("nfl")]
        public async Task<IActionResult> GetNflScoreboard(CancellationToken cancellationToken)
        {
            string endpoint = EspnEndpoints.Scoreboard(League.NFL);
            
            Console.WriteLine(endpoint);

            ScoreboardResponseDto? result = await _espnApiClient.GetAsync<ScoreboardResponseDto>(endpoint, cancellationToken);
            
            return Ok(result);
        }
    }
}