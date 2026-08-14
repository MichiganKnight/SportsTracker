using SportsTracker.Frontend.ViewModels.PlayByPlay;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class PlayByPlayMapper : IPlayByPlayMapper
    {
        public PlayByPlayViewModel Map(GamePlayByPlay playByPlay)
        {
            return new PlayByPlayViewModel
            {
                GameId = playByPlay.GameId,
                League = playByPlay.League,

                Filters = GetFilters(playByPlay.League),

                Plays = playByPlay.Plays
                    .Select(play => new GamePlayViewModel
                    {
                        Id = play.Id,
                        Type = play.Type,
                        Text = play.Text,
                        Period = play.Period,
                        Clock = play.Clock,
                        Category = play.Category ?? "other",
                        ScoringPlay = play.ScoringPlay,
                        TeamId = play.TeamId,
                        AwayScore = play.AwayScore,
                        HomeScore = play.HomeScore,
                        GroupId = play.GroupId,
                        Situation = play.Situation,
                        Context = play.Context
                    }).ToList()
            };
        }

        private static IReadOnlyList<PlayFilterViewModel> GetFilters(League league)
        {
            List<PlayFilterViewModel> filters =
            [
                new()
                {
                    Key = "all",
                    Label = "All"
                }
            ];

            switch (league)
            {
                case League.MLB:
                    filters.AddRange(
                    [
                        new PlayFilterViewModel
                        {
                            Key = "atbat",
                            Label = "At-Bats"
                        },
                        new PlayFilterViewModel
                        {
                            Key = "pitch",
                            Label = "Pitches"
                        },
                        new PlayFilterViewModel
                        {
                            Key = "scoring",
                            Label = "Scoring"
                        }
                    ]);

                    break;

                case League.NFL:
                case League.CFB:
                    filters.AddRange(
                    [
                        new PlayFilterViewModel
                        {
                            Key = "scoring",
                            Label = "Scoring"
                        },
                        new PlayFilterViewModel
                        {
                            Key = "pass",
                            Label = "Passing"
                        },
                        new PlayFilterViewModel
                        {
                            Key = "rush",
                            Label = "Rushing"
                        },
                        new PlayFilterViewModel
                        {
                            Key = "turnover",
                            Label = "Turnovers"
                        }
                    ]);

                    break;

                default:
                    filters.Add(
                        new PlayFilterViewModel
                        {
                            Key = "scoring",
                            Label = "Scoring"
                        });

                    break;
            }

            return filters;
        }
    }
}