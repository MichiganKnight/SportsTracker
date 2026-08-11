using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;
using SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class GameDetailsMapper
    {
        public static GameDetails Map(GameDetailsResponseDto response, League league)
        {
            GameDetailsCompetitionDto? competition = response.Competitions?.FirstOrDefault();

            if (competition is null)
            {
                return null;
            }

            GameDetailsCompetitorDto? away = competition.Competitors?.FirstOrDefault(x => string.Equals(x.HomeAway, "away", StringComparison.OrdinalIgnoreCase));
            GameDetailsCompetitorDto? home = competition.Competitors?.FirstOrDefault(x => string.Equals(x.HomeAway, "home", StringComparison.OrdinalIgnoreCase));

            if (away is null || home is null)
            {
                return null;
            }

            return new GameDetails
            {
                Id = response.Id ?? competition.Id ?? string.Empty,
                League = league,
                
                StartTime = competition.Date ?? response.Date ?? DateTime.MinValue,
                
                Status = competition.Status?.Type?.ShortDetail ?? response.Status?.Type?.ShortDetail ?? string.Empty,
                
                IsLive = competition.Status?.Type?.State == "in",
                
                AwayTeam = MapTeam(away),
                HomeTeam = MapTeam(home),
                
                Venue = competition.Venue?.FullName,
                VenueCity = competition.Venue?.Address?.City,
                VenuState = competition.Venue?.Address?.State,
                
                Broadcasts = MapBroadcasts(competition),
                FeaturedAthletes = MapFeaturedAthletes(competition),
                
                Headline = competition.Headlines?.FirstOrDefault()?.ShortLinkText,
                
                Recap = competition.Headlines?.FirstOrDefault()?.Description,
                
                Baseball = league == League.MLB ? MapBaseball(away, home) : null
            };
        }

        private static GameDetailsTeam MapTeam(GameDetailsCompetitorDto competitor)
        {
            GameDetailsTeamDto? team = competitor.Team;

            return new GameDetailsTeam
            {
                Id = team?.Id ?? competitor.Id ?? string.Empty,
                
                Name = team?.Name ?? string.Empty,
                
                DisplayName = team?.DisplayName ?? team?.Name ?? string.Empty,
                
                Abbreviation = team?.Abbreviation ?? string.Empty,
                
                Logo = team?.Logo,
                Color = team?.Color,
                AlternateColor = team?.AlternateColor,
                
                Score = ParseScore(competitor.Score),
                
                Winner = competitor.Winner == true,
                
                Record = GetOverallRecord(competitor),
                
                Hits = competitor.Hits,
                Errors = competitor.Errors,
                
                LineScores = MapLineScores(competitor.LineScores)
            };
        }

        private static int ParseScore(string? score)
        {
            return int.TryParse(score, out int value) ? value : 0;
        }

        private static IReadOnlyList<LineScore> MapLineScores(IReadOnlyList<LineScoreDto>? lineScores)
        {
            if (lineScores is null)
            {
                return [];
            }
            
            return lineScores
                .Where(x => x.Period.HasValue)
                .Select(x => new LineScore
                {
                    Period = x.Period!.Value,
                    Value = x.Value ?? 0,
                    DisplayValue = x.DisplayValue ?? (x.Value?.ToString() ?? string.Empty)
                })
                .OrderBy(x => x.Period)
                .ToList();
        }

        private static string? GetOverallRecord(GameDetailsCompetitorDto competitor)
        {
            RecordDto? record = competitor.Records?
                .FirstOrDefault(x => string.Equals(x.Type, "total", StringComparison.OrdinalIgnoreCase)) ?? competitor.Records?.FirstOrDefault();
            
            return record?.Summary;
        }

        private static IReadOnlyList<string> MapBroadcasts(GameDetailsCompetitionDto competition)
        {
            List<string> broadcasts = [];

            if (competition.Broadcasts is not null)
            {
                foreach (BroadcastDto broadcast in competition.Broadcasts)
                {
                    if (broadcast.Names is null)
                    {
                        continue;
                    }

                    foreach (string name in broadcast.Names)
                    {
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            broadcasts.Add(name);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(competition.Broadcast))
            {
                broadcasts.Add(competition.Broadcast);
            }
            
            return broadcasts.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        private static BaseballGameDetails MapBaseball(GameDetailsCompetitorDto away, GameDetailsCompetitorDto home)
        {
            ProbablePitcherDto? awayPitcher = away.Probables?.FirstOrDefault();
            ProbablePitcherDto? homePitcher = home.Probables?.FirstOrDefault();

            return new BaseballGameDetails
            {
                AwayProbablePitcher = MapAthlete(awayPitcher?.Athlete),
                HomeProbablePitcher = MapAthlete(homePitcher?.Athlete),

                AwayProbablePitcherRecord = awayPitcher?.Record,
                HomeProbablePitcherRecord = homePitcher?.Record
            };
        }

        private static Athlete? MapAthlete(AthleteDto? athlete)
        {
            if (athlete is null)
            {
                return null;
            }

            return new Athlete
            {
                Id = athlete.Id ?? string.Empty,
                Name = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName,
                Jersey = athlete.Jersey,
                Headshot = athlete.Headshot,
                TeamId = athlete.Team?.Id
            };
        }

        private static IReadOnlyList<FeaturedAthlete> MapFeaturedAthletes(GameDetailsCompetitionDto competition)
        {
            return [];
        }
    }
}