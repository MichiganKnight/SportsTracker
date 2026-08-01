using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Frontend.ViewModels.Shared;
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

                Status = game.StatusBadge,

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