using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Services;

namespace SportsTracker.App.Controllers
{
    [Route("golf")]
    public class GolfController(IScoreboardService scoreboardService, IGolfTournamentViewModelMapper golfTournamentViewModelMapper) : Controller
    {
        [HttpGet("{eventId}")]
        public async Task<IActionResult> Index(string eventId, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(League.PGA, cancellationToken);
            
            Game? golfEvent = scoreboard?.Games.FirstOrDefault(game => game.Id == eventId && game.Golf is not null);
            
            if (golfEvent is null)
            {
                return NotFound();
            }
            
            return View(golfTournamentViewModelMapper.Map(golfEvent));
        }
    }
}