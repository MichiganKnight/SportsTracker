using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    [Route("games")]
    public sealed class GamesController(IScoreboardService scoreboardService, IDashboardViewModelMapper dashboardViewModelMapper) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> Index(DateOnly? date, CancellationToken cancellationToken)
        {
            DateOnly selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);
            
            League[] supportedLeagues = [.. LeagueConfiguration.All];

            Task<CachedScoreboard?>[] scoreboardTasks = LeagueConfiguration.All.Select(league => scoreboardService.GetScoreboardAsync(league, selectedDate, cancellationToken)).ToArray();

            CachedScoreboard?[] scoreboards = await Task.WhenAll(scoreboardTasks);

            List<LeagueSectionViewModel> leagues = [];

            for (int i = 0; i < supportedLeagues.Length; i++)
            {
                League league = supportedLeagues[i];

                CachedScoreboard? scoreboard = scoreboards[i];
                
                LeagueSectionViewModel section = dashboardViewModelMapper.MapLeague(league, scoreboard?.Games);
                
                leagues.Add(section);
            }

            GamesPageViewModel model = new()
            {
                Date = selectedDate,

                Leagues = leagues
            };
            
            return View(model);
        }

        [HttpGet("league-section")]
        public async Task<IActionResult> LeagueSection(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);
            
            LeagueSectionViewModel viewModel = dashboardViewModelMapper.MapLeague(league, scoreboard?.Games);
            
            return PartialView("~/Views/Shared/Dashboard/_LeagueSection.cshtml", viewModel);
        }
    }
}