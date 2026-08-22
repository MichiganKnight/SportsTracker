using SportsTracker.App.Enums;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Mapping
{
    public interface IAthleteGameLogViewModelMapper
    {
        AthleteGameLogViewModel Map(AthleteGameLog gameLog, League league);
    }

    public sealed class AthleteGameLogViewModelMapper : IAthleteGameLogViewModelMapper
    {
        public AthleteGameLogViewModel Map(AthleteGameLog gameLog, League league)
        {
            return new AthleteGameLogViewModel
            {
                League = league,
                
                Columns = gameLog.Columns
                    .Select(column => new AthleteGameLogColumnViewModel
                    {
                        Label = column.Label,
                        DisplayName = column.DisplayName
                    })
                    .ToList(),

                Seasons = gameLog.Seasons
                    .OrderBy(season => GetSeasonTypeOrder(season.DisplayName))
                    .Select(season => new AthleteGameLogSeasonViewModel
                    {
                        DisplayName = season.DisplayName,
                        TeamAbbreviation = season.TeamAbbreviation,

                        Categories = season.Categories
                            .Select(category => new AthleteGameLogCategoryViewModel
                            {
                                DisplayName = category.DisplayName,

                                Games = category.Games.Select(MapGame).ToList(),

                                Totals = category.Totals
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }

        private static AthleteGameLogGameViewModel MapGame(AthleteGameLogGame game)
        {
            return new AthleteGameLogGameViewModel
            {
                EventId = game.EventId,

                GameDate = game.GameDate,

                DateDisplay = game.GameDate?.ToLocalTime().ToString("MMM d") ?? string.Empty,

                Result = game.Result,
                Score = game.Score,
                AtVs = game.AtVs,

                EventNote = game.EventNote,

                OpponentId = game.OpponentId,
                OpponentName = game.OpponentName,
                OpponentAbbreviation = game.OpponentAbbreviation,
                OpponentLogo = game.OpponentLogo,

                Stats = game.Stats
            };
        }

        private static int GetSeasonTypeOrder(string? displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                return 99;
            }

            string value = displayName.Trim();

            if (value.Contains("Regular Season", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (value.Contains("Postseason", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }

            if (value.Contains("Preseason", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            
            return 99;
        }
    }
}