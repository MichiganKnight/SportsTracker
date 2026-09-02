using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    [Route("league")]
    public sealed class LeagueLeadersController(ILeagueLeadersService leagueLeadersService, ILeagueLeadersViewModelMapper viewModelMapper) : Controller
    {
        [HttpGet("{league}/leaders")]
        public async Task<IActionResult> Index(League league, CancellationToken cancellationToken)
        {
            LeagueLeaders? leaders = await leagueLeadersService.GetLeadersAsync(league, cancellationToken);

            if (leaders is null)
            {
                return NotFound();
            }

            LeagueLeadersViewModel viewModel = viewModelMapper.Map(league, leaders);
            
            return View(viewModel);
        }
    }
}