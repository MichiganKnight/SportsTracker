using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Models.Sport;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class ScoreboardMapper
    {
        public static CachedScoreboard MapScoreboard(ScoreboardResponseDto dto, League league, DateTime updatedUtc)
        {
            ScoreboardLeagueDto? leagueInfo = dto.Leagues.FirstOrDefault();
            
            return new CachedScoreboard
            {
                League = league,
                Games = [.. ToGames(dto, league)],

                LeagueLogo = GetLeagueLogo(leagueInfo?.Logos, "default"),
                LeagueDarkLogo = GetLeagueLogo(leagueInfo?.Logos, "dark"),

                LastUpdatedUtc = updatedUtc
            };
        }
        
        public static IEnumerable<Game> ToGames(ScoreboardResponseDto dto, League league)
        {
            foreach (EventDto @event in dto.Events)
            {
                CompetitionDto? competition = @event.Competitions.FirstOrDefault();

                if (competition is null)
                {
                    continue;
                }

                if (league == League.PGA)
                {
                    yield return MapGolfEvent(@event, competition, league);
                    
                    continue;
                }

                CompetitorDto? home = competition.Competitors.FirstOrDefault(c => c.HomeAway == "home");
                CompetitorDto? away = competition.Competitors.FirstOrDefault(c => c.HomeAway == "away");

                if (home is null || away is null)
                {
                    continue;
                }
                
                yield return MapTeamEvent(@event, competition, home, away, league);
            }
        }

        private static Game MapTeamEvent(EventDto @event, CompetitionDto competition, CompetitorDto home, CompetitorDto away, League league)
        {
            return new Game
            {
                Id = @event.Id,
                League = league,
                StartTime = @event.Date,
                Status = MapStatus(competition.Status.Type.Name),
                StatusText = competition.Status.Type.ShortDetail,

                Situation = SituationMapper.Map(competition, league),

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

        private static Game MapGolfEvent(EventDto @event, CompetitionDto competition, League league)
        {
            return new Game
            {
                Id = @event.Id,
                League = league,
                StartTime = @event.Date,
                Status = MapStatus(competition.Status.Type.Name),
                StatusText = competition.Status.Type.ShortDetail,

                Golf = new GolfTournament
                {
                    Name = @event.Name,
                    EndTime = @event.EndDate,

                    Leaderboard = competition.Competitors
                        .Where(competitor => competitor.Athlete is not null)
                        .OrderBy(competitor => competitor.Order ?? int.MaxValue)
                        .Select(MapGolfer)
                        .ToList()
                }
            };
        }

        private static GolfLeaderboardEntry MapGolfer(CompetitorDto competitor)
        {
            GolfAthleteDto athlete = competitor.Athlete!;

            return new GolfLeaderboardEntry
            {
                AthleteId = competitor.Id ?? string.Empty,
                Name = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName ?? athlete.DisplayName ?? athlete.FullName ?? string.Empty,

                CountryFlag = athlete.Flag?.Href,
                Country = athlete.Flag?.Alt,

                Position = competitor.Order,
                ScoreToPar = competitor.Score ?? string.Empty,

                Rounds = competitor.LineScores
                    .Where(round => round.Period.HasValue)
                    .OrderBy(round => round.Period)
                    .Select(MapGolfRound)
                    .ToList()
            };
        }

        private static GolfRound MapGolfRound(GolfLineScoreDto dto)
        {
            return new GolfRound
            {
                Round = dto.Period ?? 0,
                Strokes = dto.Value.HasValue ? Convert.ToInt32(dto.Value.Value) : null,
                ScoreToPar = dto.DisplayValue ?? string.Empty,

                Holes = dto.LineScores
                    .Where(hole => hole.Period.HasValue)
                    .OrderBy(hole => hole.Period)
                    .Select(MapGolfHole)
                    .ToList()
            };
        }

        private static GolfHole MapGolfHole(GolfLineScoreDto dto)
        {
            return new GolfHole
            {
                Hole = dto.Period ?? 0,
                Strokes = dto.Value.HasValue ? Convert.ToInt32(dto.Value.Value) : null,
                ScoreToPar = dto.ScoreType?.DisplayValue ?? string.Empty,
            };
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
                Logo = !string.IsNullOrEmpty(competitor.Team.Logo) ? new Logo(competitor.Team.Logo,500, 500, $"{competitor.Team.Logo} Logo") : competitor.Team.Logos.Select(MapLogo).FirstOrDefault(),
                Record = competitor.Records.Select(MapRecord).FirstOrDefault()
            };
        }

        private static Logo? MapLogo(EspnLogoDto dto)
        {
            return new Logo(dto.Href, dto.Width, dto.Height, dto.Alt);
        }

        private static Record? MapRecord(RecordDto dto)
        {
            return new Record(dto.Summary, dto.DisplayValue);
        }
        
        private static string? GetLeagueLogo(IReadOnlyList<EspnLogoDto>? logos, string relation)
        {
            if (logos is null)
            {
                return null;
            }
            
            return logos.FirstOrDefault(logo => logo.Rel.Any(rel => rel.Equals(relation, StringComparison.OrdinalIgnoreCase)))?.Href;
        }
    }
}