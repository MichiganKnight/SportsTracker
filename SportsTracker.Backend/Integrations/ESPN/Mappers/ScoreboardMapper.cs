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

                CompetitorDto home = competition.Competitors.First(c => c.HomeAway == "home");
                CompetitorDto away = competition.Competitors.First(c => c.HomeAway == "away");

                yield return new Game
                {
                    Id = @event.Id,
                    League = league,
                    StartTime = @event.Date,
                    Status = MapStatus(competition.Status.Type.Name),
                    StatusText = competition.Status.Type.ShortDetail,

                    HomeTeam = MapTeam(home.Team, league, home),
                    AwayTeam = MapTeam(away.Team, league, away),

                    HomeScore = int.TryParse(home.Score, out int hs) ? hs : 0,
                    AwayScore = int.TryParse(away.Score, out int ascore) ? ascore : 0,

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
    }
}