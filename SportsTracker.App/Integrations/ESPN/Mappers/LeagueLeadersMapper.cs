using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class LeagueLeadersMapper
    {
        public static LeaderCategory MapCategory(LeagueLeadersResponseDto response, string categoryName, string statisticName)
        {
            LeagueLeaderCategoryMetadataDto? metadata = response.Categories.FirstOrDefault(category => string.Equals(category.Name, categoryName, StringComparison.OrdinalIgnoreCase));

            int statisticIndex  = FindStatisticIndex(metadata, statisticName);

            if (statisticIndex < 0)
            {
                return new LeaderCategory
                {
                    Name = statisticName,
                    DisplayName = statisticName,
                    Abbreviation = statisticName,

                    Leaders = []
                };
            }

            string displayName = GetAt(metadata?.DisplayNames, statisticIndex) ?? statisticName;
            string abbreviation = GetAt(metadata?.Labels, statisticIndex) ?? statisticName;

            List<StatLeader> leaders = response.Athletes?
                .Select((entry, index) => MapLeader(entry, categoryName, statisticIndex, index + 1))
                .Where(leader => leader is not null)
                .Select(leader => leader!)
                .ToList() ?? [];

            return new LeaderCategory
            {
                Name = statisticName,
                DisplayName = displayName,
                Abbreviation = abbreviation,

                Leaders = leaders
            };
        }

        private static StatLeader? MapLeader(LeagueLeaderAthleteEntryDto entry, string categoryName, int statisticIndex, int rank)
        {
            LeagueLeaderAthleteDto? athlete = entry.Athlete;

            if (athlete is null)
            {
                return null;
            }

            LeagueLeaderAthleteCategoryDto? category = entry.Categories.FirstOrDefault(item => string.Equals(item.Name, categoryName, StringComparison.OrdinalIgnoreCase));

            if (category is null)
            {
                return null;
            }
            
            string? displayValue = GetAt(category.Totals, statisticIndex);
            double? value = GetAt(category.Values, statisticIndex);

            if (value is null)
            {
                return null;
            }

            return new StatLeader
            {
                Rank = rank,

                DisplayValue = displayValue ?? value.Value.ToString(),
                Value = value.Value,

                AthleteId = athlete.Id ?? string.Empty,

                AthleteName = athlete.DisplayName ?? athlete.ShortName ?? string.Empty,
                Headshot = athlete.Headshot?.Href,

                TeamId = athlete.TeamId ?? string.Empty,

                TeamName = athlete.TeamName ?? athlete.TeamShortName ?? string.Empty,
                TeamAbbreviation = athlete.TeamShortName ?? string.Empty,

                TeamLogo = GetPrimaryLogo(athlete.TeamLogos)
            };
        }

        private static int FindStatisticIndex(LeagueLeaderCategoryMetadataDto? category, string statisticName)
        {
            if (category?.Names is null)
            {
                return -1;
            }

            for (int i = 0; i < category.Names.Count; i++)
            {
                if (string.Equals(category.Names[i], statisticName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            
            return -1;
        }

        private static string? GetPrimaryLogo(IReadOnlyList<EspnLogoDto>? logos)
        {
            if (logos is null || logos.Count == 0)
            {
                return null;
            }
            
            EspnLogoDto? defaultLogo = logos.FirstOrDefault(logo => logo.Rel.Any(rel => string.Equals(rel, "default", StringComparison.OrdinalIgnoreCase)));
            
            return defaultLogo?.Href ?? logos[0].Href;
        }

        private static T? GetAt<T>(IReadOnlyList<T>? values, int index)
        {
            if (values is null || index < 0 || index >= values.Count)
            {
                return default;
            }
            
            return values[index];
        }
    }
}