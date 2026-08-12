using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    public sealed class BoxScoreController : Controller
    {
        private readonly IBoxScoreService _boxScoreService;
        
        public BoxScoreController(IBoxScoreService boxScoreService)
        {
            _boxScoreService = boxScoreService;
        }

        [HttpGet("{league}/{gameId}/boxscore")]
        public async Task<ActionResult<ApiResponse<GameBoxScore>>> GetBoxScore(League league, string gameId, CancellationToken cancellationToken)
        {
            GameBoxScore? boxScore = await _boxScoreService.GetBoxScoreAsync(league, gameId, cancellationToken);
            
            if (boxScore is null)
            {
                return NotFound();
            }
            
            return Ok(new ApiResponse<GameBoxScore>
            {
                Data = boxScore,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}