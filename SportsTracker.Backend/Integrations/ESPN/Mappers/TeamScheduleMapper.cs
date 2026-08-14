using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class TeamScheduleMapper
    {
        public static TeamSchedule Map(TeamScheduleResponseDto dto, League league, string teamId)
        {
            return new TeamSchedule
            {
                TeamId = teamId,
                League = league,

                Games = dto.Events
                    .Select(@event => MapGame(@event, league))
                    .Where(game => game is not null)
                    .Select(game => game!)
                    .OrderBy(game => game.StartTime)
                    .ToList()
            };
        }

        private static Game? MapGame(TeamScheduleEventDto @event, League league)
        {
            TeamScheduleCompetitionDto? competition = @event.Competitions.FirstOrDefault();

            if (competition is null)
            {
                return null;
            }

            TeamScheduleCompetitorDto? home = competition.Competitors.FirstOrDefault(competitor => string.Equals(competitor.HomeAway, "home", StringComparison.OrdinalIgnoreCase));
            TeamScheduleCompetitorDto? away = competition.Competitors.FirstOrDefault(competitor => string.Equals(competitor.HomeAway, "away", StringComparison.OrdinalIgnoreCase));

            if (home is null || away is null)
            {
                return null;
            }

            return new Game
            {
                Id = @event.Id,
                League = league,
                StartTime = @event.Date,

                Status = MapStatus(competition.Status?.Type.Name),

                StatusText = competition.Status?.Type.ShortDetail ?? competition.Status?.Type.Detail ?? string.Empty,

                HomeTeam = MapTeam(home, league),
                AwayTeam = MapTeam(away, league),

                HomeScore = ParseScore(home.Score),
                AwayScore = ParseScore(away.Score),

                Venue = competition.Venue is null
                    ? null : new Venue
                    {
                        Name = competition.Venue.FullName ?? string.Empty
                    },

                IsNeutralSite = competition.NeutralSite
            };
        }

        private static Team MapTeam(TeamScheduleCompetitorDto competitor, League league)
        {
            TeamScheduleRecordDto? overallRecord = competitor.Record.FirstOrDefault(record => string.Equals(record.Type, "total", StringComparison.OrdinalIgnoreCase));

            return new Team
            {
                Id = competitor.Team.Id,
                League = league,

                Location = competitor.Team.Location,

                Name = competitor.Team.ShortDisplayName ?? competitor.Team.DisplayName ?? string.Empty,

                DisplayName = competitor.Team.DisplayName ?? competitor.Team.ShortDisplayName ?? string.Empty,

                Abbreviation = competitor.Team.Abbreviation ?? string.Empty,

                Logo = competitor.Team.Logos
                    .Where(logo => logo.Rel.Contains("default", StringComparer.OrdinalIgnoreCase))
                    .Select(MapLogo)
                    .FirstOrDefault() ?? competitor.Team.Logos.Select(MapLogo).FirstOrDefault(),

                Record = overallRecord is null ? null : new Record(overallRecord.DisplayValue ?? string.Empty, overallRecord.DisplayValue)
            };
        }
        
        private static int ParseScore(TeamScheduleScoreDto? score)
        {
            if (score is null)
            {
                return 0;
            }

            if (int.TryParse(score.DisplayValue, out int displayValue))
            {
                return displayValue;
            }
            
            return score.Value.HasValue ? (int)score.Value.Value : 0;
        }

        private static GameStatus MapStatus(string? statusName)
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

        private static Logo? MapLogo(TeamLogoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Href))
            {
                return null;
            }

            return new Logo(dto.Href, dto.Width ?? 0, dto.Height ?? 0, dto.Alt ?? string.Empty);
        }
    }
}