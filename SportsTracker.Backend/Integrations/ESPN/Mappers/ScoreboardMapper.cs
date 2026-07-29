using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class ScoreboardMapper
    {
        public static IEnumerable<Game> ToGames(ScoreboardResponseDto dto, League league)
        {
            foreach (EventDto @event in dto.Events)
            {
                CompetitionDto? competition = @event.Competitions.FirstOrDefault();

                if (competition is null)
                {
                    continue;
                }

                CompetitorDto? home = competition.Competitors.FirstOrDefault(c => c.HomeAway == "home");
                CompetitorDto? away = competition.Competitors.FirstOrDefault(c => c.HomeAway == "away");

                if (home is null || away is null)
                {
                    continue;
                }

                yield return new Game
                {
                    Id = @event.Id,
                    League = league,
                    StartTime = @event.Date,
                    Status = MapStatus(competition.Status.Type.Name),
                    StatusText = competition.Status.Type.ShortDetail,

                    HomeTeam = MapTeam(home, league),
                    AwayTeam = MapTeam(away, league),

                    HomeScore = int.TryParse(home.Score, out int homeScore) ? homeScore : 0,
                    AwayScore = int.TryParse(away.Score, out int awayScore) ? awayScore : 0,

                    Venue = competition.Venue is null
                        ? null
                        : new Venue
                        {
                            Id = competition.Venue.Id,
                            Name = competition.Venue.FullName,
                            IsIndoor = competition.Venue.Indoor
                        },

                    IsNeutralSite = @event.NeutralSite
                };
            }
        }

        private static GameStatus MapStatus(string statusName)
        {
            return statusName switch
            {
                "STATUS_SCHEDULED" => GameStatus.Scheduled,
                "STATUS_IN_PROGRESS" => GameStatus.InProgress,
                "STATUS_HALFTIME" => GameStatus.Halftime,
                "STATUS_FINAL" => GameStatus.Final,
                "STATUS_POSTPONED" => GameStatus.Postponed,
                "STATUS_DELAYED" => GameStatus.Delayed,
                "STATUS_CANCELLED" => GameStatus.Cancelled,
                _ => GameStatus.Scheduled
            };
        }

        private static Team MapTeam(CompetitorDto competitor, League league)
        {
            return new Team
            {
                Id = competitor.Team.Id,
                League = league,
                Location = competitor.Team.Location,
                Nickname = competitor.Team.Nickname,
                Name = competitor.Team.Name,
                DisplayName = competitor.Team.DisplayName,
                Abbreviation = competitor.Team.Abbreviation,
                Color = competitor.Team.Color,
                AlternateColor = competitor.Team.AlternateColor,
                Logo = competitor.Team.Logos.Select(MapLogo).FirstOrDefault(),
                Record = competitor.Records.Select(MapRecord).FirstOrDefault()
            };
        }

        private static Logo? MapLogo(LogoDto dto)
        {
            return new Logo(dto.Href, dto.Width, dto.Height, dto.Alt);
        }

        private static Record? MapRecord(RecordDto dto)
        {
            return new Record(dto.Summary, dto.DisplayValue);
        }
    }
}