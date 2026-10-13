using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Models;
using SportsTracker.App.Models.Groups;
using SportsTracker.App.Models.Season;
using SportsTracker.App.Models.Standings;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.Season;
using SportsTracker.App.ViewModels.Standings;

namespace SportsTracker.App.Controllers
{
    public sealed class StandingsController(IStandingsService standingsService, IGroupsService groupsService, ISeasonService seasonService, IScoreboardService scoreboardService, IStandingsViewModelMapper standingsViewModelMapper, IPostseasonViewModelMapper postseasonViewModelMapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(League league, StandingsView view, int? season, int? seasonType, int? round, CancellationToken cancellationToken = default)
        {
            SeasonContext seasonContext = seasonService.Resolve(league, season, seasonType);
            
            SeasonSelectorViewModel seasonSelector = CreateSeasonSelector(league, seasonContext, view);

            if (league == League.NFL && seasonContext.Phase == SeasonPhase.Postseason)
            {
                return await Postseason(league, seasonContext, seasonSelector, round, cancellationToken);
            }
            
            Task<LeagueStandings?> standingsTask = standingsService.GetStandingsAsync(league, seasonContext.Season, seasonContext.SeasonType, cancellationToken);
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
            
            viewModel.SeasonSelector = seasonSelector;
            
            return View(viewModel);
        }

        private SeasonSelectorViewModel CreateSeasonSelector(League league, SeasonContext seasonContext, StandingsView view)
        {
            return new SeasonSelectorViewModel
            {
                League = league,

                SelectedSeason = seasonContext.Season,
                SelectedSeasonType = seasonContext.SeasonType,

                Seasons = seasonService.GetAvailableSeasons(league),
                Phases = seasonService.GetAvailablePhases(league),

                Controller = "Standings",
                Action = "Index",

                RouteValues = new Dictionary<string, string>
                {
                    ["league"] = league.ToString(),
                    ["view"] = view.ToString()
                }
            };
        }

        private async Task<IActionResult> Postseason(League league, SeasonContext seasonContext, SeasonSelectorViewModel seasonSelector, int? round, CancellationToken cancellationToken)
        {
            CachedScoreboard? seasonScoreboard = await scoreboardService.GetScoreboardAsync(league, seasonContext.Season, seasonContext.SeasonType, cancellationToken);

            if (seasonScoreboard?.Calendar is null)
            {
                return NotFound();
            }

            SeasonPeriod? postseason = seasonScoreboard.Calendar.Periods.FirstOrDefault(period => period.SeasonType == seasonContext.SeasonType);

            if (postseason is null || postseason.Weeks.Count == 0)
            {
                return NotFound();
            }

            IReadOnlyList<SeasonWeek> rounds = postseason.Weeks.Where(IsPlayoffRound).ToList();

            if (rounds.Count == 0)
            {
                return NotFound();
            }
            
            SeasonWeek selectedRound = rounds.FirstOrDefault(week => week.Number == round) ?? rounds[0];

            CachedScoreboard? roundScoreboard = await scoreboardService.GetScoreboardAsync(league, seasonContext.Season, seasonContext.SeasonType, selectedRound.Number, cancellationToken);

            if (roundScoreboard is null)
            {
                return NotFound();
            }

            SeasonPeriod playoffPeriod = new()
            {
                SeasonType = postseason.SeasonType,
                Name = postseason.Name,
                StartDate = postseason.StartDate,
                EndDate = postseason.EndDate,
                Weeks = rounds
            };
            
            PostseasonViewModel viewModel = postseasonViewModelMapper.Map(roundScoreboard, playoffPeriod, selectedRound.Number, seasonSelector);
            
            return View("Postseason", viewModel);
        }

        private static bool IsPlayoffRound(SeasonWeek week)
        {
            return !week.Name.Contains("Pro Bowl", StringComparison.OrdinalIgnoreCase) && !week.AlternateName.Contains("Pro Bowl", StringComparison.OrdinalIgnoreCase);
        }
    }
}