using SportsTracker.Frontend.ViewModels.GameInfo;
using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class GameCardMapper : IGameCardMapper
    {
        public GameCardViewModel Map(Game game)
        {
            return new GameCardViewModel
            {
                GameId = game.Id,
                League = game.League,

                AwayTeam = MapTeam(game.AwayTeam, game.AwayScore),
                HomeTeam = MapTeam(game.HomeTeam, game.HomeScore),

                Status = game.StatusText,
                
                SituationHeadline = game.Situation?.Headline,
                SituationSubheadline = game.Situation?.Subheadline,
                SituationDetail = game.Situation?.Detail,
                
                Baseball = game.Situation?.Baseball,

                IsLive = game.IsLive,
                IsFinal = game.IsFinal,
                IsUpcoming = game.IsUpcoming,

                Venue = game.Venue?.Name,
                StartTime = game.StartTime,
                IsNeutralSite = game.IsNeutralSite
            };
        }
        
        private TeamViewModel MapTeam(Team team, int score)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                League = team.League,
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
    }
}