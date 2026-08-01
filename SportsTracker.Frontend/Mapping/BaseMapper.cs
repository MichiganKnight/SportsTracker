using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public abstract class BaseMapper
    {
        protected TeamViewModel MapTeam(Team team, int score)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Abbreviation,
                DisplayName = team.DisplayName,
                Abbreviation = team.Abbreviation,
                Logo = team.Logo?.Href,
                Record = team.Record?.Summary,
                Score = score,
                PrimaryColor = team.Color,
                AlternateColor = team.AlternateColor
            };
        }

        protected GameCardViewModel MapGame(Game game)
        {
            return new GameCardViewModel
            {
                GameId = game.Id,

                AwayTeam = MapTeam(game.AwayTeam, game.AwayScore),
                HomeTeam = MapTeam(game.HomeTeam, game.HomeScore),

                Status = game.StatusText,

                IsLive = game.IsLive,
                IsFinal = game.IsFinal,
                IsUpcoming = game.IsUpcoming,

                Venue = game.Venue?.Name,
                StartTime = game.StartTime,
                IsNeutralSite = game.IsNeutralSite
            };
        }
    }
}