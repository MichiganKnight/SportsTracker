using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/scoreboard")]
    public class ScoreboardController : ControllerBase
    {
        private readonly IScoreboardService _scoreboardService;
        
        public ScoreboardController(IScoreboardService scoreboardService)
        {
            _scoreboardService = scoreboardService;
        }

        [HttpGet("{league}")]
        public async Task<IActionResult> GetScoreboard(League league, CancellationToken cancellationToken)
        {
            IReadOnlyList<Game> games = await _scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            return Ok(games);
        }
    }
}