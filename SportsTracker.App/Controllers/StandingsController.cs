using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.Groups;
using SportsTracker.App.Models.Standings;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.Standings;

namespace SportsTracker.App.Controllers
{
    public sealed class StandingsController(IStandingsService standingsService, IGroupsService groupsService, IStandingsViewModelMapper standingsViewModelMapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(League league, StandingsView view, CancellationToken cancellationToken = default)
        {
            Task<LeagueStandings?> standingsTask = standingsService.GetStandingsAsync(league, cancellationToken);
            Task<IReadOnlyList<SportsGroup>?> groupsTask = groupsService.GetGroupsAsync(league, cancellationToken);
            
            await Task.WhenAll(standingsTask, groupsTask);
            
            LeagueStandings? standings = await standingsTask;
            IReadOnlyList<SportsGroup>? groups = await groupsTask;

            if (standings is null)
            {
                return NotFound();
            }

            if (groups is not null)
            {
                standings = StandingsGrouping.AddDivisionGroups(standings, groups);
            }
            
            StandingsViewModel viewModel = standingsViewModelMapper.Map(standings, view, DateTime.UtcNow);
            
            return View(viewModel);
        }
    }
}