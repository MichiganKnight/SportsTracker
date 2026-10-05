using SportsTracker.App.Enums;
using SportsTracker.App.Models.TeamInfo;
using SportsTracker.App.ViewModels.TeamInfo;

namespace SportsTracker.App.Mapping
{
    public interface ITeamStatsViewModelMapper
    {
        TeamStatsViewModel Map(TeamDetailsViewModel team, TeamStats stats, string? section = null);
    }

    public sealed class TeamStatsViewModelMapper : ITeamStatsViewModelMapper
    {
        private static readonly string[] FootballSections =
        [
            "Overview",
            "Passing",
            "Rushing",
            "Receiving",
            "Defense",
            "Scoring"
        ];

        private static readonly string[] BaseballSections =
        [
            "Overview",
            "Batting",
            "Pitching",
            "Fielding"
        ];

        public TeamStatsViewModel Map(TeamDetailsViewModel team, TeamStats stats, string? section = null)
        {
            IReadOnlyList<string> availableSections = GetAvailableSections(team.League);

            string selectedSection = ResolveSection(section, availableSections);

            IReadOnlyList<TeamStatSectionViewModel> sections = selectedSection.Equals("Overview", StringComparison.OrdinalIgnoreCase) ? BuildOverview(team.League, stats) : BuildSection(stats, selectedSection);

            return new TeamStatsViewModel
            {
                Team = team,
                Season = stats.Season,
                SeasonType = stats.SeasonType,

                SelectedSection = selectedSection,
                AvailableSections = availableSections,

                Sections = sections
            };
        }

        private static IReadOnlyList<string> GetAvailableSections(League league)
        {
            return league switch
            {
                League.NFL or League.CFB => FootballSections,
                League.MLB => BaseballSections,

                _ => ["Overview"]
            };
        }

        private static string ResolveSection(string? requestedSection, IReadOnlyList<string> availableSections)
        {
            if (string.IsNullOrWhiteSpace(requestedSection))
            {
                return "Overview";
            }

            return availableSections.FirstOrDefault(available => available.Equals(requestedSection, StringComparison.OrdinalIgnoreCase)) ?? "Overview";
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildOverview(League league, TeamStats stats)
        {
            return league switch
            {
                League.NFL or League.CFB => BuildFootballOverview(stats),
                League.MLB => BuildBaseballOverview(stats),

                _ => []
            };
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildFootballOverview(TeamStats stats)
        {
            TeamStatSectionViewModel[] allSections =
            [
                CreateCuratedSection(stats, "passing", "Passing", ["passingYards", "passingYardsPerGame", "passingTouchdowns", "completionPct", "QBRating"]),
                CreateCuratedSection(stats, "rushing", "Rushing", ["rushingYards", "rushingYardsPerGame", "rushingTouchdowns", "yardsPerRushAttempt"]),
                CreateCuratedSection(stats, "receiving", "Receiving", ["receivingYards", "receivingYardsPerGame", "receivingTouchdowns", "receptions"]),
                CreateCuratedSection(stats, "defensive", "Defense", ["totalTackles", "sacks", "tacklesForLoss", "passesDefended"])
            ];

            return allSections.Where(section => section.Stats.Count > 0).ToList();
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildBaseballOverview(TeamStats stats)
        {
            TeamStatSectionViewModel[] allSections =
            [
                CreateCuratedSection(stats, "batting", "Batting", ["avg", "hits", "homeRuns", "runs", "RBIs", "onBasePct", "slugAvg", "OPS"]),
                CreateCuratedSection(stats, "pitching", "Pitching", ["ERA", "wins", "losses", "strikeouts", "walks", "WHIP", "saves"])
            ];

            return allSections.Where(section => section.Stats.Count > 0).ToList();
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildSection(TeamStats stats, string selectedSection)
        {
            TeamStatCategory? category = stats.Categories.FirstOrDefault(category => category.Name.Equals(selectedSection, StringComparison.OrdinalIgnoreCase) || category.DisplayName.Equals(selectedSection, StringComparison.OrdinalIgnoreCase));

            if (category is null)
            {
                return [];
            }

            return
            [
                MapSection(category)
            ];
        }

        private static TeamStatSectionViewModel CreateCuratedSection(TeamStats stats, string categoryName, string displayName, IReadOnlyList<string> statisticNames)
        {
            TeamStatCategory? category = stats.Categories.FirstOrDefault(category => category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category is null)
            {
                return new TeamStatSectionViewModel
                {
                    Name = displayName,
                    DisplayName = displayName
                };
            }

            List<TeamStatisticViewModel> selectedStats = [];

            foreach (string statisticName in statisticNames)
            {
                TeamStatistic? statistic = category.Stats.FirstOrDefault(stat => stat.Name.Equals(statisticName, StringComparison.OrdinalIgnoreCase));

                if (statistic is not null)
                {
                    selectedStats.Add(MapStatistic(statistic));
                }
            }

            return new TeamStatSectionViewModel
            {
                Name = category.Name,
                DisplayName = displayName,
                Summary = category.Summary,
                Stats = selectedStats
            };
        }

        private static TeamStatSectionViewModel MapSection(TeamStatCategory category)
        {
            return new TeamStatSectionViewModel
            {
                Name = category.Name,
                DisplayName = category.DisplayName,
                Summary = category.Summary,

                Stats = category.Stats.Select(MapStatistic).ToList()
            };
        }

        private static TeamStatisticViewModel MapStatistic(TeamStatistic statistic)
        {
            return new TeamStatisticViewModel
            {
                Name = statistic.Name,
                DisplayName = statistic.DisplayName,
                Abbreviation = statistic.Abbreviation,
                Value = statistic.DisplayValue,
                Rank = statistic.RankDisplayValue
            };
        }
    }
}