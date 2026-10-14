using System.Text.Json;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models.Sport;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class GameSituationDetailsMapper
    {
        private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
        
        public static FootballSituation? Football(JsonElement? situation) => Deserialize<FootballSituation>(situation);

        public static IReadOnlyList<FootballPlaybackPlay> HistoricalFootballPlays(GameSummaryResponseDto summary)
        {
            List<FootballPlaybackPlay> result = [];

            if (summary.Drives is null)
            {
                return result;
            }

            List<DriveDto> drives = summary.Drives.Previous?.ToList() ?? [];

            if (summary.Drives.Current is not null)
            {
                drives.Add(summary.Drives.Current);
            }

            int driveNumber = 0;

            foreach (DriveDto drive in drives)
            {
                driveNumber++;

                if (drive.Plays is null)
                {
                    continue;
                }

                foreach (PlayDto play in drive.Plays)
                {
                    FootballSituation? situation = MapHistoricalPlaySituation(play, drive.Team?.Id);
                    
                    result.Add(new FootballPlaybackPlay
                    {
                        Id = play.Id ?? $"{driveNumber}-{result.Count}", Index = result.Count, DriveNumber = driveNumber,

                        Quarter = play.Period?.Number, Clock = play.Clock?.DisplayValue,

                        Description = play.Text ?? play.Type?.Text ?? "Play Description Unavailable",

                        PlayType = play.Type?.Text,

                        AwayScore = play.AwayScore,
                        HomeScore = play.HomeScore,

                        IsScoringPlay = play.ScoringPlay == true,

                        Situation = situation
                    });
                }
            }
            
            return result;
        }

        private static FootballSituation? MapHistoricalPlaySituation(PlayDto play, string? driveTeamId)
        {
            PlaySituationDto? start = play.Start;

            if (start is null)
            {
                return null;
            }
            
            int? down = start.Down;
            int? distance = start.Distance;
            int? yardsToEndzone = start.YardsToEndzone;

            if (down is null or < 1 or > 4 || distance is null or < 0 || yardsToEndzone is null or < 0 or > 100)
            {
                return null;
            }
            
            string? possessionId = start.Team?.Id ?? play.Team?.Id ?? driveTeamId;
            
            string downLabel = down.Value switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                4 => "4th",
                
                _ => $"{down}th"
            };

            string distanceLabel = distance == 0 ? "Goal" : distance.Value.ToString();
            string description = $"{downLabel} & {distanceLabel}";

            return new FootballSituation
            {
                Down = down,
                Distance = distance,

                YardLine = start.YardLine,
                YardsToEndzone = yardsToEndzone,

                PossessionTeamId = possessionId,
                PossessionText = start.PossessionText,

                DownDistanceText = start.DownDistanceText ?? description,

                ShortDownDistanceText = start.ShortDownDistanceText ?? description,

                IsRedZone = yardsToEndzone <= 20
            };
        }

        public static BaseballSituation? Baseball(JsonElement? situation)
        {
            BaseballSituationDto? dto = Deserialize<BaseballSituationDto>(situation);

            if (dto is null)
            {
                return null;
            }

            return new BaseballSituation
            {
                Balls = dto.Balls, Strikes = dto.Strikes, Outs = dto.Outs,
                OnFirst = dto.OnFirst, OnSecond = dto.OnSecond, OnThird = dto.OnThird,
                LastPlay = dto.LastPlay?.Text
            };
        }

        private static T? Deserialize<T>(JsonElement? situation)
        {
            if (situation is null || situation.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            {
                return default;
            }
            
            return situation.Value.Deserialize<T>(Options);
        }
    }
}