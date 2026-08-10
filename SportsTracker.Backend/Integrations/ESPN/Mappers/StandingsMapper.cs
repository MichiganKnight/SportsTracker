using SportsTracker.Backend.Integrations.ESPN.DTOs.Standings;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.Standings;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class StandingsMapper
    {
        public static LeagueStandings Map(StandingsResponseDto response, League league)
        {
            List<StandingsGroup> groups = response.Children?
                .Where(group => group.Standings is not null)
                .Select(group => MapGroup(group, league))
                .ToList() ?? [];

            int season = response.Children?
                .Select(group => group.Standings?.Season)
                .FirstOrDefault(value => value.HasValue) ?? DateTime.UtcNow.Year;

            return new LeagueStandings
            {
                League = league,
                Season = season,
                Groups = groups
            };
        }

        private static StandingsGroup MapGroup(StandingsGroupDto dto, League league)
        {
            List<TeamStanding> teams = dto.Standings?.Entries?
                .Where(entry => entry.Team is not null)
                .Select(MapTeam)
                .ToList() ?? [];

            return new StandingsGroup
            {
                Id = dto.Id ?? string.Empty,
                Name = dto.Name ?? string.Empty,
                Abbreviation = dto.Abbreviation ?? string.Empty,
                Type = GetGroupType(league),
                Teams = teams
            };
        }

        private static TeamStanding MapTeam(StandingsEntryDto entry)
        {
            StandingTeamDto team = entry.Team!;

            return new TeamStanding
            {
                TeamId = team.Id ?? string.Empty,
                Name = team.DisplayName ?? team.Name ?? string.Empty,
                Abbreviation = team.Abbreviation ?? string.Empty,
                Logo = GetPrimaryLogo(team),

                Wins = GetIntStat(entry, "wins"),
                Losses = GetIntStat(entry, "losses"),
                WinPercentage = GetDoubleStat(entry, "winpercent"),
                GamesBack = GetNullableDoubleStat(entry, "gamesbehind"),

                RunsScored = GetNullableIntStat(entry, "pointsfor"),
                RunsAllowed = GetNullableIntStat(entry, "pointsagainst"),
                RunDifferential = GetNullableIntStat(entry, "pointdifferential"),

                Streak = GetDisplayStat(entry, "streak"),
                PlayoffSeed = GetNullableIntStat(entry, "playoffSeed")
            };
        }

        private static StandingsGroupType GetGroupType(League league)
        {
            return league switch
            {
                League.MLB => StandingsGroupType.League,
                League.NFL or League.NBA or League.NHL => StandingsGroupType.Conference,
                League.CFB or League.CBB => StandingsGroupType.Conference,
                _ => StandingsGroupType.Overall
            };
        }

        private static string? GetPrimaryLogo(StandingTeamDto team)
        {
            return team.Logos?.FirstOrDefault()?.Href;
        }

        private static StandingStatDto? GetStat(StandingsEntryDto entry, string type)
        {
            return entry.Stats?.FirstOrDefault(stat =>
                string.Equals(stat.Type, type, StringComparison.OrdinalIgnoreCase));
        }

        private static int GetIntStat(StandingsEntryDto entry, string type)
        {
            return GetNullableIntStat(entry, type) ?? 0;
        }

        private static int? GetNullableIntStat(StandingsEntryDto entry, string type)
        {
            double? value = GetNullableDoubleStat(entry, type);

            return value.HasValue
                ? Convert.ToInt32(value.Value)
                : null;
        }

        private static double GetDoubleStat(StandingsEntryDto entry, string type)
        {
            return GetNullableDoubleStat(entry, type) ?? 0;
        }

        private static double? GetNullableDoubleStat(StandingsEntryDto entry, string type)
        {
            return GetStat(entry, type)?.Value;
        }

        private static string? GetDisplayStat(StandingsEntryDto entry, string type)
        {
            return GetStat(entry, type)?.DisplayValue;
        }
    }
}