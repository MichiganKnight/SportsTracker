using SportsTracker.App.Enums;
using SportsTracker.App.Models.Groups;
using SportsTracker.App.Models.Standings;

namespace SportsTracker.App.Services
{
    public static class StandingsGrouping
    {
        public static LeagueStandings AddDivisionGroups(LeagueStandings standings, IReadOnlyList<SportsGroup> groups)
        {
            Dictionary<string, TeamStanding> teamsById = standings.Groups
                .SelectMany(group => group.Teams)
                .GroupBy(team => team.TeamId)
                .ToDictionary(group => group.Key, group => group.First());

            List<StandingsGroup> divisionGroups = groups
                .SelectMany(group => group.Children)
                .Where(group => group.TeamIds.Count > 0)
                .Select(group => MapDivisionGroup(group, teamsById))
                .Where(group => group.Teams.Count > 0)
                .ToList();

            return new LeagueStandings
            {
                League = standings.League,
                Season = standings.Season,

                Groups = standings.Groups.Concat(divisionGroups).ToList()
            };
        }

        private static StandingsGroup MapDivisionGroup(SportsGroup group, IReadOnlyDictionary<string, TeamStanding> teamsById)
        {
            List<TeamStanding> teams = group.TeamIds
                .Where(teamsById.ContainsKey)
                .Select(teamId => teamsById[teamId])
                .OrderBy(team => team.GamesBack ?? double.MaxValue)
                .ThenByDescending(team => team.WinPercentage)
                .ToList();
            
            return new StandingsGroup
            {
                Id = group.Abbreviation,
                Name = group.Name,
                Abbreviation = group.Abbreviation,
                Type = StandingsGroupType.Division,
                Teams = teams
            };
        }
    }
}