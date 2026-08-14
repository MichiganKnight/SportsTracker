using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Controllers
{
    [Route("team")]
    public sealed class TeamController(ITeamApiClient api, ITeamDetailsMapper teamDetailsMapper, IGameCardMapper gameCardMapper) : Controller
    {
        [HttpGet("{league}/{teamId}")]
        public async Task<IActionResult> Index(League league, string teamId, CancellationToken cancellationToken)
        {
            ApiResponse<TeamDetails>? response = await api.GetTeamDetailsAsync(league, teamId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            TeamDetailsViewModel viewModel = teamDetailsMapper.Map(response.Data);
            
            return View(viewModel);
        }

        [HttpGet("{league}/{teamId}/schedule")]
        public async Task<IActionResult> Schedule(League league, string teamId, CancellationToken cancellationToken)
        {
            Task<ApiResponse<TeamDetails>?> teamTask = api.GetTeamDetailsAsync(league, teamId, cancellationToken);
            Task<ApiResponse<TeamSchedule>?> scheduleTask = api.GetTeamScheduleAsync(league, teamId, cancellationToken);
            
            await Task.WhenAll(teamTask, scheduleTask);
            
            ApiResponse<TeamDetails>? teamResponse = await teamTask;
            ApiResponse<TeamSchedule>? scheduleResponse = await scheduleTask;

            if (teamResponse?.Data is null || scheduleResponse?.Data is null)
            {
                return NotFound();
            }

            TeamDetailsViewModel team = teamDetailsMapper.Map(teamResponse.Data);

            IReadOnlyList<GameCardViewModel> games = scheduleResponse.Data.Games.Select(gameCardMapper.Map).ToList();
            
            return View(new TeamSchedulePageViewModel
            {
                Team = team,
                Games = games
            });
        }
    }
}