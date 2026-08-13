using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Controllers
{
    [Route("team")]
    public sealed class TeamController(ITeamApiClient api, ITeamDetailsMapper mapper) : Controller
    {
        [HttpGet("{league}/{teamId}")]
        public async Task<IActionResult> Index(League league, string teamId, CancellationToken cancellationToken)
        {
            ApiResponse<TeamDetails>? response = await api.GetTeamDetailsAsync(league, teamId, cancellationToken);

            if (response?.Data is null)
            {
                return NotFound();
            }
            
            TeamDetailsViewModel viewModel = mapper.Map(response.Data);
            
            return View(viewModel);
        }
    }
}