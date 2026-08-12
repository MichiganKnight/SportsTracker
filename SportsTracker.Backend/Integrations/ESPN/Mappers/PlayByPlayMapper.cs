using SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore;
using SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class PlayByPlayMapper
    {
        public static GamePlayByPlay Map(GameSummaryResponseDto response, string gameId, League league)
        {
            if (response?.Plays is null)
            {
                return null;
            }

            List<GamePlay> plays = response.Plays.Where(ShouldIncludePlay).Select(MapPlay).ToList();

            return new GamePlayByPlay
            {
                GameId = gameId,
                League = league,
                Plays = plays
            };
        }

        private static GamePlay MapPlay(PlayDto dto)
        {
            return new GamePlay
            {
                Id = dto.Id ?? string.Empty,

                Type = dto.Type?.Type ?? dto.Type?.Text,

                Text = dto.Text,

                ShortText = dto.Type?.AlternativeText,

                Period = FormatPeriod(dto.Period),

                Clock = null,

                SequenceNumber = ParseSequenceNumber(dto.SequenceNumber),
                
                AwayScore = dto.AwayScore,
                HomeScore = dto.HomeScore,

                ScoringPlay = dto.ScoringPlay == true,

                TeamId = dto.Team?.Id
            };
        }

        private static string? FormatPeriod(PlayPeriodDto? period)
        {
            if (period is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(period.DisplayValue))
            {
                return period.Type switch
                {
                    "Top" => $"Top {period.Number}",
                    "Bottom" => $"Bottom {period.Number}",

                    _ => period.DisplayValue
                };
            }

            if (period.Number.HasValue)
            {
                return period.Type is null ? period.Number.Value.ToString() : $"{period.Type} {period.Number.Value}";
            }

            return period.Type;
        }

        private static int? ParseSequenceNumber(string? sequenceNumber)
        {
            return int.TryParse(sequenceNumber, out int value) ? value : null;
        }

        private static bool ShouldIncludePlay(PlayDto play)
        {
            return !string.IsNullOrWhiteSpace(play.Text);
        }
    }
}