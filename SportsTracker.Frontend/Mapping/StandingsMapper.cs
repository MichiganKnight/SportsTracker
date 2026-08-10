using SportsTracker.Frontend.ViewModels.Standings;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class StandingsMapper : IStandingsMapper
    {
        public StandingsViewModel Map(LeagueStandings standings, DateTime? lastUpdatedUtc = null)
        {
            return new StandingsViewModel
            {
                League = standings.League,
                LeagueName = LeagueConfiguration.Get(standings.League).DisplayName,
                Season = standings.Season,
                LastUpdatedUtc = lastUpdatedUtc,
                Groups = standings.Groups.Select(MapGroup).ToList()
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
                WinPercentage = team.WinPercentage,
                GamesBack = team.GamesBack,

                RunsScored = team.RunsScored,
                RunsAllowed = team.RunsAllowed,
                RunDifferential = team.RunDifferential,

                Streak = team.Streak,
                PlayoffSeed = team.PlayoffSeed
            };
        }
    }
}