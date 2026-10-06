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
        private sealed record StatSectionDefinition(string Category, string DisplayName, IReadOnlyList<string> Stats);

        private static readonly StatSectionDefinition FootballPassing = new("passing", "Passing",
        [
            "completions",
            "passingAttempts",
            "completionPct",
            "passingYards",
            "passingYardsPerGame",
            "yardsPerPassAttempt",
            "longPassing",
            "passingTouchdowns",
            "interceptions",
            "sacks",
            "sackYardsLost",
            "QBR",
            "QBRating"
        ]);

        private static readonly StatSectionDefinition FootballRushing = new("rushing", "Rushing",
        [
            "rushingAttempts",
            "rushingYards",
            "rushingYardsPerGame",
            "yardsPerRushAttempt",
            "longRushing",
            "rushingBigPlays",
            "rushingTouchdowns",
            "rushingFumbles",
            "rushingFumblesLost",
            "rushingFirstDowns"
        ]);

        private static readonly StatSectionDefinition FootballReceiving = new("receiving", "Receiving",
        [
            "receptions",
            "receivingTargets",
            "receivingYards",
            "receivingYardsPerGame",
            "yardsPerReception",
            "receivingBigPlays",
            "receivingTouchdowns",
            "receivingYardsAfterCatch",
            "receivingFirstDowns",
            "receivingFumbles",
            "receivingFumblesLost"
        ]);

        private static readonly StatSectionDefinition FootballDefense = new("defensive", "Defense",
        [
            "soloTackles",
            "assistTackles",
            "totalTackles",
            "sacks",
            "sackYards",
            "tacklesForLoss",
            "passesDefended"
        ]);

        private static readonly StatSectionDefinition FootballDefensiveInterceptions = new("defensiveinterceptions", "Interceptions",
        [
            "interceptions",
            "interceptionYards",
            "interceptionTouchdowns"
        ]);

        private static readonly StatSectionDefinition FootballScoring = new("scoring", "Scoring",
        [
            "totalPoints",
            "totalPointsPerGame",
            "totalTouchdowns",
            "passingTouchdowns",
            "rushingTouchdowns",
            "receivingTouchdowns",
            "returnTouchdowns",
            "fieldGoals",
            "kickExtraPoints",
            "totalTwoPointConvs"
        ]);

        private static readonly StatSectionDefinition BaseballBatting = new("batting", "Batting",
        [
            "gamesPlayed",
            "atBats",
            "runs",
            "hits",
            "avg",
            "doubles",
            "triples",
            "homeRuns",
            "RBIs",
            "totalBases",
            "walks",
            "strikeouts",
            "stolenBases",
            "onBasePct",
            "slugAvg",
            "OPS"
        ]);

        private static readonly StatSectionDefinition BaseballPitching = new("pitching", "Pitching",
        [
            "gamesPlayed",
            "gamesStarted",
            "qualityStarts",
            "ERA",
            "wins",
            "losses",
            "saves",
            "holds",
            "innings",
            "hits",
            "earnedRuns",
            "homeRuns",
            "walks",
            "strikeouts",
            "strikeoutsPerNineInnings",
            "WHIP"
        ]);

        private static readonly StatSectionDefinition BaseballFielding = new("fielding", "Fielding",
        [
            "gamesPlayed",
            "putouts",
            "assists",
            "errors",
            "fieldingPct",
            "doublePlays"
        ]);

        private static readonly IReadOnlyList<StatSectionDefinition> FootballSections =
        [
            FootballPassing,
            FootballRushing,
            FootballReceiving,
            FootballDefense,
            FootballScoring
        ];

        private static readonly IReadOnlyList<StatSectionDefinition> BaseballSections =
        [
            BaseballBatting,
            BaseballPitching,
            BaseballFielding
        ];

        public TeamStatsViewModel Map(TeamDetailsViewModel team, TeamStats stats, string? section = null)
        {
            IReadOnlyList<StatSectionDefinition> definitions = GetDefinitions(team.League);

            IReadOnlyList<string> availableSections =
            [
                "Overview",
                .. definitions.Select(definition => definition.DisplayName)
            ];

            string selectedSection = ResolveSection(section, availableSections);

            IReadOnlyList<TeamStatSectionViewModel> sections = selectedSection.Equals("Overview", StringComparison.OrdinalIgnoreCase) ? BuildOverview(team.League, stats) : BuildSelectedSection(stats, selectedSection, definitions);

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

        private static IReadOnlyList<StatSectionDefinition> GetDefinitions(League league)
        {
            return league switch
            {
                League.NFL or League.CFB => FootballSections,
                League.MLB => BaseballSections,

                _ => []
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
            List<TeamStatSectionViewModel> sections =
            [
                CreateSection(stats, FootballPassing,
                [
                    "passingYards",
                    "passingYardsPerGame",
                    "passingTouchdowns",
                    "completionPct"
                ]),

                CreateSection(stats, FootballRushing,
                [
                    "rushingYards",
                    "rushingYardsPerGame",
                    "yardsPerRushAttempt",
                    "rushingTouchdowns"
                ]),

                CreateSection(stats, FootballReceiving,
                [
                    "receptions",
                    "receivingYards",
                    "receivingYardsPerGame",
                    "receivingTouchdowns"
                ]),

                CreateSection(stats, FootballScoring,
                [
                    "totalPoints",
                    "totalPointsPerGame",
                    "totalTouchdowns",
                    "fieldGoals"
                ])
            ];

            return sections.Where(section => section.Stats.Count > 0).ToList();
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildBaseballOverview(TeamStats stats)
        {
            List<TeamStatSectionViewModel> sections =
            [
                CreateSection(stats, BaseballBatting,
                [
                    "avg",
                    "runs",
                    "hits",
                    "homeRuns",
                    "RBIs",
                    "onBasePct",
                    "slugAvg",
                    "OPS"
                ]),

                CreateSection(stats, BaseballPitching,
                [
                    "ERA",
                    "wins",
                    "losses",
                    "strikeouts",
                    "saves",
                    "WHIP"
                ])
            ];

            return sections.Where(section => section.Stats.Count > 0).ToList();
        }

        private static IReadOnlyList<TeamStatSectionViewModel> BuildSelectedSection(TeamStats stats, string selectedSection, IReadOnlyList<StatSectionDefinition> definitions)
        {
            StatSectionDefinition? definition = definitions.FirstOrDefault(definition => definition.DisplayName.Equals(selectedSection, StringComparison.OrdinalIgnoreCase));

            if (definition is null)
            {
                return [];
            }
            
            TeamStatSectionViewModel section = CreateSection(stats, definition);

            if (section.Stats.Count == 0)
            {
                return [];
            }

            if (selectedSection.Equals("Defense", StringComparison.OrdinalIgnoreCase))
            {
                TeamStatSectionViewModel interceptions = CreateSection(stats, FootballDefensiveInterceptions);
                
                return interceptions.Stats.Count > 0 ? [section, interceptions] : [section];
            }
            
            return [section];
        }

        private static TeamStatSectionViewModel CreateSection(TeamStats stats, StatSectionDefinition definition, IReadOnlyList<string>? selectedStats  = null)
        {
            TeamStatCategory? category = FindCategory(stats, definition.Category);

            if (category is null)
            {
                return new TeamStatSectionViewModel
                {
                    Name = definition.Category,
                    DisplayName = definition.DisplayName
                };
            }

            IReadOnlyList<string> requestedStats = selectedStats ?? definition.Stats;

            List<TeamStatisticViewModel> mappedStats = [];

            foreach (string statName in requestedStats)
            {
                TeamStatistic? statistic = category.Stats.FirstOrDefault(stat => stat.Name.Equals(statName, StringComparison.OrdinalIgnoreCase));

                if (statistic is not null)
                {
                    mappedStats.Add(MapStatistic(statistic));
                }
            }

            return new TeamStatSectionViewModel
            {
                Name = category.Name,
                DisplayName = definition.DisplayName,
                Summary = category.Summary,
                Stats = mappedStats
            };
        }

        private static TeamStatCategory? FindCategory(TeamStats stats, string categoryName)
        {
            return stats.Categories.FirstOrDefault(category => category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
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