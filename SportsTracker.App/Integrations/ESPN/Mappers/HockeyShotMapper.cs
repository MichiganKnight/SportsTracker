using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models.Sport;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class HockeyShotMapper
    {
        public static IReadOnlyList<HockeyShot> Map(GameSummaryResponseDto summary)
        {
            if (summary.Plays is null)
            {
                return [];
            }

            return summary.Plays.Where(play => play.Coordinate?.X is not null && play.Coordinate.Y is not null && play.Period?.Number is not null && IsShotEvent(play.Type?.Abbreviation))
                .Select(play => new HockeyShot
                {
                    Id = play.Id ?? string.Empty,
                    Description = play.Text,
                    TeamId = play.Team?.Id,
                    Clock = play.Clock?.DisplayValue,
                    Strength = play.Strength?.Text,

                    Period = play.Period!.Number!.Value,

                    X = play.Coordinate!.X!.Value,
                    Y = play.Coordinate.Y!.Value,

                    IsGoal = play.Type?.Abbreviation == "goal",
                    IsSaved = play.Type?.Abbreviation == "shot-on-goal",
                    IsMissed = play.Type?.Abbreviation == "shot-missed"
                })
                .ToList();
        }

        private static bool IsShotEvent(string? type)
        {
            return type is "goal" or "shot-on-goal" or "shot-missed";
        }
    }
}