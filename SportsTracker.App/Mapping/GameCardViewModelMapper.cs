using SportsTracker.App.Models;
using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.ViewModels.GameInfo;
using SportsTracker.App.ViewModels.TeamInfo;

namespace SportsTracker.App.Mapping
{
    public interface IGameCardViewModelMapper
    {
        GameCardViewModel Map(Game game);
    }
    
    public sealed class GameCardViewModelMapper : IGameCardViewModelMapper
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
                Football = game.Situation?.Football,

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