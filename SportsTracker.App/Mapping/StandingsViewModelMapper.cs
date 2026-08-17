using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models.Standings;
using SportsTracker.App.ViewModels.Standings;

namespace SportsTracker.App.Mapping
{
    public interface IStandingsViewModelMapper
    {
        StandingsViewModel Map(LeagueStandings standings, StandingsView view, DateTime? lastUpdatedUtc = null);
    }
    
    public sealed class StandingsViewModelMapper : IStandingsViewModelMapper
    {
        public StandingsViewModel Map(LeagueStandings standings, StandingsView view, DateTime? lastUpdatedUtc = null)
        {
            IReadOnlyList<StandingsView> availableViews = GetAvailableViews(standings.League);

            return new StandingsViewModel
            {
                League = standings.League,
                LeagueName = LeagueConfiguration.Get(standings.League).DisplayName,
                Season = standings.Season,
                LastUpdatedUtc = lastUpdatedUtc,
                SelectedView = view,
                AvailableViews = availableViews,
                ShowTies = standings.League is League.NFL,
                ShowGamesBack = standings.League is League.MLB or League.NBA,
                ShowDifferential = standings.League is League.NFL or League.MLB,
                ShowStreak = true,
                Groups = MapGroups(standings, view)
            };
        }

        private static IReadOnlyList<StandingsView> GetAvailableViews(League league)
        {
            return league switch
            {
                League.MLB =>
                [
                    StandingsView.Overall,
                    StandingsView.League,
                    StandingsView.Division
                ],

                League.NFL or League.NBA or League.NHL =>
                [
                    StandingsView.Overall,
                    StandingsView.Conference,
                    StandingsView.Division
                ],

                League.CFB =>
                [
                    StandingsView.Overall,
                    StandingsView.Conference
                ],

                _ =>
                [
                    StandingsView.Overall
                ]
            };
        }

        private static IReadOnlyList<StandingsGroupViewModel> MapGroups(LeagueStandings standings, StandingsView view)
        {
            return view switch
            {
                StandingsView.Overall => [MapOverallGroup(standings)],
                StandingsView.League => standings.Groups.Where(group => group.Type == StandingsGroupType.League).Select(MapGroup).ToList(),
                StandingsView.Conference => standings.Groups.Where(group => group.Type == StandingsGroupType.Conference).Select(MapGroup).ToList(),
                StandingsView.Division => standings.Groups.Where(group => group.Type == StandingsGroupType.Division).Select(MapGroup).ToList(),
                _ => []
            };
        }

        private static StandingsGroupViewModel MapOverallGroup(LeagueStandings standings)
        {
            List<TeamStandingViewModel> teams = standings.Groups
                .SelectMany(group => group.Teams)
                .GroupBy(team => team.TeamId)
                .Select(group => group.First())
                .OrderByDescending(team => team.WinPercentage)
                .ThenByDescending(team => team.Wins)
                .Select(MapTeam)
                .ToList();
            
            return new StandingsGroupViewModel
            {
                Id = "overall",
                Name = "Overall",
                Abbreviation = "Overall",
                Type = StandingsGroupType.Overall,
                Teams = teams
            };
        }
        
        private static StandingsGroupViewModel  MapGroup(StandingsGroup group)
        {
            return new StandingsGroupViewModel
            {
                Name = group.Name,
                Abbreviation = group.Abbreviation,
                Teams = group.Teams.Select(MapTeam).ToList()
            };
        }

        private static TeamStandingViewModel MapTeam(TeamStanding team)
        {
            return new TeamStandingViewModel
            {
                TeamId = team.TeamId,
                Name = team.Name,
                Abbreviation = team.Abbreviation,
                Logo = team.Logo,

                Wins = team.Wins,
                Losses = team.Losses,
                Ties = team.Ties,
                
                WinPercentage = team.WinPercentage,
                GamesBack = team.GamesBack,

                PointsFor = team.PointsFor,
                PointsAgainst = team.PointsAgainst,
                PointDifferential = team.PointDifferential,

                Streak = team.Streak,
                PlayoffSeed = team.PlayoffSeed
            };
        }
    }
}