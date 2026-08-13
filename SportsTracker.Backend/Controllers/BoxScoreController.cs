using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Backend.Controllers
{
    [Route("api/v1/games")]
    public sealed class BoxScoreController(IBoxScoreService boxScoreService) : ApiControllerBase
    {
        [HttpGet("{league}/{gameId}/boxscore")]
        public async Task<ActionResult<ApiResponse<GameBoxScore>>> GetBoxScore(League league, string gameId, CancellationToken cancellationToken)
        {
            GameBoxScore? boxScore = await boxScoreService.GetBoxScoreAsync(league, gameId, cancellationToken);
            
            return boxScore is null ? NotFound() : ApiOk(boxScore);
        }
    }
}