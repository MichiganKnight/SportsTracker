using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class LeagueLeadersMapper
    {
        public static LeagueLeaders Map(LeagueLeadersResponseDto response)
        {
            List<LeaderCategory> categories = response.Stats?.Categories?.Select(MapCategory).ToList() ?? [];
            
            return new LeagueLeaders
            {
                Season = response.Season?.Year ?? DateTime.UtcNow.Year,
                
                SeasonName = response.Season?.DisplayName ?? response.Season?.Year?.ToString() ?? string.Empty,
                
                Categories = categories
            };
        }

        private static LeaderCategory MapCategory(LeagueLeaderCategoryDto category)
        {
            List<StatLeader> leaders = category.Leaders?.Select((leader, index) => MapLeader(leader, category.Name, index + 1)).ToList() ?? [];
            
            return new LeaderCategory
            {
                Name = category.Name ?? string.Empty,
                DisplayName = category.DisplayName ?? category.Name ?? string.Empty,
                Abbreviation = category.Abbreviation ?? string.Empty,
                
                Leaders = leaders
            };
        }

        private static StatLeader MapLeader(LeagueLeaderDto leader, string? statisticName, int rank)
        {
            LeagueLeaderTeamDto? team = leader.Team ?? leader.Athlete?.Team;

            return new StatLeader
            {
                Rank = rank,

                Value = leader.Value ?? 0,

                DisplayValue = GetStatisticDisplayValue(leader, statisticName),

                AthleteId = leader.Athlete?.Id ?? string.Empty,

                AthleteName = leader.Athlete?.DisplayName ?? leader.Athlete?.ShortName ?? string.Empty,
                Headshot = leader.Athlete?.Headshot?.Href,

                TeamId = team?.Id ?? string.Empty,
                TeamName = team?.DisplayName ?? team?.Name ?? string.Empty,
                TeamAbbreviation = team?.Abbreviation ?? string.Empty,

                TeamLogo = GetPrimaryLogo(team)
            };
        }

        private static string GetStatisticDisplayValue(LeagueLeaderDto leader, string? statisticName)
        {
            if (!string.IsNullOrWhiteSpace(statisticName))
            {
                LeagueLeaderStatDto? statistic = leader.Statistics?.Splits?.Categories?
                    .SelectMany(category => category.Stats ?? [])
                    .FirstOrDefault(stat => string.Equals(stat.Name, statisticName, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(statistic?.DisplayValue))
                {
                    return statistic.DisplayValue;
                }
            }
            
            return leader.Value?.ToString() ?? string.Empty;
        }

        private static string? GetPrimaryLogo(LeagueLeaderTeamDto? team)
        {
            if (team?.Logos is null)
            {
                return null;
            }

            EspnLogoDto? defaultLogo = team.Logos.FirstOrDefault(logo => logo.Rel.Any(rel => string.Equals(rel, "default", StringComparison.OrdinalIgnoreCase)));
            
            return defaultLogo?.Href ?? team.Logos.FirstOrDefault()?.Href;
        }
    }
}