using SportsTracker.Frontend.ViewModels.GameDetails;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models.GameDetails;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class GameDetailsMapper : IGameDetailsMapper
    {
        public GameDetailsViewModel Map(GameDetails details)
        {
            string? location = BuildLocation(details.VenueCity, details.VenuState);

            return new GameDetailsViewModel
            {
                GameId = details.Id,
                League = details.League,

                LeagueName = LeagueConfiguration.Get(details.League).DisplayName,

                StartTime = details.StartTime,

                Status = details.Status,

                IsLive = details.IsLive,
                IsFinal = details.IsFinal,

                AwayTeam = MapTeam(details.AwayTeam),
                HomeTeam = MapTeam(details.HomeTeam),

                Venue = details.Venue,
                Location = location,

                Attendance = details.Attendance,
                Broadcasts = details.Broadcasts,

                FeaturedAthletes = details.FeaturedAthletes.Select(MapFeaturedAthlete).ToList(),

                Headline = details.Headline,
                Recap = details.Recap,

                Baseball = details.Baseball is null ? null : MapBaseball(details.Baseball)
            };
        }

        private static GameDetailsTeamViewModel MapTeam(GameDetailsTeam team)
        {
            return new GameDetailsTeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                DisplayName = team.DisplayName,
                Abbreviation = team.Abbreviation,

                Logo = team.Logo,

                PrimaryColor = team.Color,
                AlternateColor = team.AlternateColor,

                Score = team.Score,
                Winner = team.Winner,
                
                Record = team.Record,

                Hits = team.Hits,
                Errors = team.Errors,

                LineScores = team.LineScores.Select(x => new LineScoreViewModel
                {
                    Period = x.Period,
                    DisplayValue = x.DisplayValue
                }).ToList()
            };
        }

        private static FeaturedAthleteViewModel MapFeaturedAthlete(FeaturedAthlete athlete)
        {
            return new FeaturedAthleteViewModel
            {
                Type = athlete.Type,
                Label = athlete.Label,

                Name = athlete.Athlete.Name,
                ShortName = athlete.Athlete.ShortName,
                Headshot = athlete.Athlete.Headshot,

                TeamId = athlete.TeamId
            };
        }

        private static BaseballGameDetailsViewModel MapBaseball(BaseballGameDetails baseball)
        {
            return new BaseballGameDetailsViewModel
            {
                AwayProbablePitcherName = baseball.AwayProbablePitcher?.ShortName ?? baseball.AwayProbablePitcher?.Name,
                AwayProbablePitcherRecord = baseball.AwayProbablePitcherRecord,
                HomeProbablePitcherName = baseball.HomeProbablePitcher?.ShortName ?? baseball.HomeProbablePitcher?.Name,
                HomeProbablePitcherRecord = baseball.HomeProbablePitcherRecord
            };
        }

        private static string? BuildLocation(string? city, string? state)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return state;
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                return city;
            }
            
            return $"{city}, {state}";
        }
    }
}