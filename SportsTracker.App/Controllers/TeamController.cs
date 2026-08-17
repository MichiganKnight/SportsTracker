using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models.TeamInfo;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.TeamInfo;

namespace SportsTracker.App.Controllers
{
    [Route("team")]
    public sealed class TeamController(ITeamDetailsService teamDetailsService, ITeamScheduleService teamScheduleService, ITeamRosterService teamRosterService, ITeamDetailsViewModelMapper teamDetailsViewModelMapper, IGameCardViewModelMapper gameCardViewModelMapper, ITeamRosterViewModelMapper teamRosterViewModelMapper) : Controller
    {
        [HttpGet("{league}/{teamId}")]
        public async Task<IActionResult> Index(League league, string teamId, CancellationToken cancellationToken)
        {
            TeamDetails? details = await teamDetailsService.GetTeamDetailsAsync(league, teamId, cancellationToken);

            if (details is null)
            {
                return NotFound();
            }
            
            TeamDetailsViewModel viewModel = teamDetailsViewModelMapper.Map(details);
            
            return View(viewModel);
        }

        [HttpGet("{league}/{teamId}/schedule")]
        public async Task<IActionResult> Schedule(League league, string teamId, CancellationToken cancellationToken)
        {
            Task<TeamDetails?> teamTask = teamDetailsService.GetTeamDetailsAsync(league, teamId, cancellationToken);
            Task<TeamSchedule?> scheduleTask = teamScheduleService.GetTeamScheduleAsync(league, teamId, cancellationToken);
            
            await Task.WhenAll(teamTask, scheduleTask);
            
            TeamDetails? details = await teamTask;
            TeamSchedule? schedule = await scheduleTask;

            if (details is null || schedule is null)
            {
                return NotFound();
            }

            TeamDetailsViewModel team = teamDetailsViewModelMapper.Map(details);
            IReadOnlyList<GameCardViewModel> games = schedule.Games.Select(gameCardViewModelMapper.Map).ToList();
            
            return View(new TeamSchedulePageViewModel
            {
                Team = team,
                Games = games
            });
        }

        [HttpGet("{league}/{teamId}/roster")]
        public async Task<IActionResult> Roster(League league, string teamId, CancellationToken cancellationToken)
        {
            Task<TeamDetails?> teamTask = teamDetailsService.GetTeamDetailsAsync(league, teamId, cancellationToken);
            Task<TeamRoster?> rosterTask = teamRosterService.GetTeamRosterAsync(league, teamId, cancellationToken);

            await Task.WhenAll(teamTask, rosterTask);

            TeamDetails? details = await teamTask;
            TeamRoster? roster = await rosterTask;

            if (details is null || roster is null)
            {
                return NotFound();
            }

            TeamDetailsViewModel team = teamDetailsViewModelMapper.Map(details);
            TeamRosterPageViewModel viewModel = teamRosterViewModelMapper.Map(team, roster);
            
            return View(viewModel);
        }
    }
}