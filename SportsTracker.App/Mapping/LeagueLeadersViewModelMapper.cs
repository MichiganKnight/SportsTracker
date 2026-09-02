using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface ILeagueLeadersViewModelMapper
    {
        LeagueLeadersViewModel Map(League league, LeagueLeaders leagueLeaders);
    }

    public sealed class LeagueLeadersViewModelMapper : ILeagueLeadersViewModelMapper
    {
        private static readonly string[] MlbBattingCategories =
        [
            "avg",
            "homeRuns",
            "RBIs",
            "runs",
            "OPS",
            "onBasePct",
            "slugAvg",
            "stolenBases",
            "hits",
            "WARBR"
        ];

        private static readonly string[] MlbPitchingCategories =
        [
            "ERA",
            "wins",
            "strikeouts",
            "saves",
            "WHIP",
            "qualityStarts",
            "opponentAvg",
            "holds",
            "avgGameScore"
        ];

        public LeagueLeadersViewModel Map(League league, LeagueLeaders leaders)
        {
            return new LeagueLeadersViewModel
            {
                League = league,
                LeagueName = LeagueConfiguration.Get(league).DisplayName,
                Season = leaders.Season,
                SeasonName = leaders.SeasonName,

                Sections = MapSections(league, leaders.Categories)
            };
        }

        private static IReadOnlyList<LeaderSectionViewModel> MapSections(League league, IReadOnlyList<LeaderCategory> categories)
        {
            return league switch
            {
                League.MLB => MapMlbSections(categories),

                _ =>
                [
                    new LeaderSectionViewModel
                    {
                        Title = "Leaders",
                        Categories = categories.Select(MapCategory).ToList()
                    }
                ]
            };
        }

        private static IReadOnlyList<LeaderSectionViewModel> MapMlbSections(IReadOnlyList<LeaderCategory> categories)
        {
            return
            [
                new LeaderSectionViewModel
                {
                    Title = "Batting",
                    Categories = SelectCategories(categories, MlbBattingCategories)
                },

                new LeaderSectionViewModel
                {
                    Title = "Pitching",
                    Categories = SelectCategories(categories, MlbPitchingCategories)
                }
            ];
        }

        private static IReadOnlyList<LeaderCategoryViewModel> SelectCategories(IReadOnlyList<LeaderCategory> categories, IReadOnlyList<string> categoryNames)
        {
            List<LeaderCategoryViewModel> result = [];

            foreach (string categoryName in categoryNames)
            {
                LeaderCategory? category = categories.FirstOrDefault(category => string.Equals(category.Name, categoryName, StringComparison.OrdinalIgnoreCase));

                if (category is not null)
                {
                    result.Add(MapCategory(category));
                }
            }
            
            return result;
        }

        private static LeaderCategoryViewModel MapCategory(LeaderCategory category)
        {
            return new LeaderCategoryViewModel
            {
                Name = category.Name,
                DisplayName = category.DisplayName,
                Abbreviation = category.Abbreviation,

                Leaders = category.Leaders.Take(5).Select(MapLeader).ToList()
            };
        }

        private static LeaderRowViewModel MapLeader(StatLeader leader)
        {
            return new LeaderRowViewModel
            {
                Rank = leader.Rank,
                DisplayValue = leader.DisplayValue,

                AthleteId = leader.AthleteId,
                AthleteName = leader.AthleteName,
                Headshot = leader.Headshot,

                TeamId = leader.TeamId,
                TeamName = leader.TeamName,
                TeamAbbreviation = leader.TeamAbbreviation,

                TeamLogo = leader.TeamLogo
            };
        }
    }
}