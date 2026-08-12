using Microsoft.AspNetCore.Mvc;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    public sealed class PlayByPlayController : Controller
    {
        private readonly IPlayByPlayService _playByPlayService;
        
        public PlayByPlayController(IPlayByPlayService playByPlayService)
        {
            _playByPlayService = playByPlayService;
        }

        [HttpGet("{league}/{gameId}/playbyplay")]
        public async Task<ActionResult<ApiResponse<GamePlayByPlay>>> GetPlayByPlay(League league, string gameId, CancellationToken cancellationToken)
        {
            GamePlayByPlay? playByPlay = await _playByPlayService.GetPlayByPlayAsync(league, gameId, cancellationToken);

            if (playByPlay is null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse<GamePlayByPlay>
            {
                Data = playByPlay,
                TimestampUtc = DateTime.UtcNow,
                Version = "v1"
            });
        }
    }
}