using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.DashboardInfo;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Controllers
{
    public class DashboardController(IScoreboardService scoreboardService, IDashboardViewModelMapper dashboardViewModelMapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            League[] leagues = LeagueConfiguration.All.OrderBy(league => LeagueConfiguration.Get(league).DisplayOrder).ToArray();

            Task<CachedScoreboard?>[] scoreboardTasks = leagues.Select(league => scoreboardService.GetScoreboardAsync(league, cancellationToken)).ToArray();

            CachedScoreboard?[] scoreboards = await Task.WhenAll(scoreboardTasks);

            int liveEvents = scoreboards.Where(scoreboard => scoreboard is not null).Sum(scoreboard => scoreboard!.Games.Count(game => game.IsLive));

            DashboardOverviewViewModel model = new()
            {
                LiveEvents = liveEvents,

                Leagues = leagues
                    .Select(league =>
                    {
                        LeagueInfo info = LeagueConfiguration.Get(league);

                        return new DashboardLeagueSummaryViewModel
                        {
                            League = league,
                            LeagueName = info.DisplayName,
                            Icon = info.Icon
                        };
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Following()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Live()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Leagues(CancellationToken cancellationToken)
        {
            League[] leagues = LeagueConfiguration.All.OrderBy(league => LeagueConfiguration.Get(league).DisplayOrder).ToArray();

            Task<CachedScoreboard?>[] scoreboardTasks = leagues.Select(league => scoreboardService.GetScoreboardAsync(league, cancellationToken)).ToArray();

            CachedScoreboard?[] scoreboards = await Task.WhenAll(scoreboardTasks);

            Dictionary<League, IReadOnlyList<Game>> gamesByLeague = new();

            for (int i = 0; i < leagues.Length; i++)
            {
                gamesByLeague[leagues[i]] = scoreboards[i]?.Games ?? [];
            }

            DashboardViewModel model = dashboardViewModelMapper.Map(gamesByLeague);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LeagueSection(League league, CancellationToken cancellationToken)
        {
            CachedScoreboard? scoreboard = await scoreboardService.GetScoreboardAsync(league, cancellationToken);

            LeagueSectionViewModel viewModel = dashboardViewModelMapper.MapDashboardLeague(league, scoreboard?.Games);

            return PartialView("~/Views/Shared/Dashboard/_LeagueSection.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllGames(CancellationToken cancellationToken)
        {
            League[] leagues = LeagueConfiguration.All.OrderBy(league => LeagueConfiguration.Get(league).DisplayOrder).ToArray();

            Task<CachedScoreboard?>[] scoreboardTasks = leagues.Select(league => scoreboardService.GetScoreboardAsync(league, cancellationToken)).ToArray();

            CachedScoreboard?[] scoreboards = await Task.WhenAll(scoreboardTasks);

            List<LeagueSectionViewModel> sections = [];

            for (int i = 0; i < leagues.Length; i++)
            {
                League league = leagues[i];
                CachedScoreboard? scoreboard = scoreboards[i];

                LeagueSectionViewModel section = dashboardViewModelMapper.MapLeague(league, scoreboard?.Games);

                sections.Add(section);
            }

            return PartialView("~/Views/Dashboard/_AllGames.cshtml", sections);
        }
    }
}