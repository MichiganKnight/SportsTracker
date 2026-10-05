using SportsTracker.App.Integrations.ESPN.DTOs.Team;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class TeamStatsMapper
    {
        public static TeamStats? Map(TeamStatsResponseDto response, int season, int seasonType)
        {
            if (response.Splits is null)
            {
                return null;
            }

            return new TeamStats
            {
                Season = season,
                SeasonType = seasonType,

                Categories = response.Splits.Categories.Select(MapCategory).ToList()
            };
        }

        private static TeamStatCategory MapCategory(TeamStatCategoryDto category)
        {
            return new TeamStatCategory
            {
                Name = category.Name ?? string.Empty,
                DisplayName = category.DisplayName ?? category.Name ?? string.Empty,
                ShortDisplayName = category.ShortDisplayName ?? category.Name ?? string.Empty,
                Abbreviation = category.Abbreviation ?? string.Empty,
                Summary = category.Summary ?? string.Empty,

                Stats = category.Stats.Select(MapStatistic).ToList()
            };
        }

        private static TeamStatistic MapStatistic(TeamStatisticDto statistic)
        {
            return new TeamStatistic
            {
                Name = statistic.Name ?? string.Empty,
                DisplayName = statistic.DisplayName ?? statistic.Name ?? string.Empty,
                ShortDisplayName = statistic.ShortDisplayName ?? statistic.Name ?? string.Empty,
                Abbreviation = statistic.Abbreviation ?? string.Empty,
                
                Value = statistic.Value ?? 0,
                DisplayValue = statistic.DisplayValue ?? string.Empty,
                
                Rank = statistic.Rank,
                RankDisplayValue = statistic.RankDisplayValue
            };
        }
    }
}