using SportsTracker.Frontend.ViewModels.PlayByPlay;
using SportsTracker.Shared.Models.PlayByPlay;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class PlayByPlayMapper : IPlayByPlayMapper
    {
        public PlayByPlayViewModel Map(GamePlayByPlay playByPlay)
        {
            return new PlayByPlayViewModel
            {
                GameId = playByPlay.GameId,
                League = playByPlay.League,

                Plays = playByPlay.Plays
                    .Select(play => new GamePlayViewModel
                    {
                        Id = play.Id,
                        Type = play.Type,
                        Text = play.Text,
                        Period = play.Period,
                        Clock = play.Clock,
                        Category = play.Category ?? "other",
                        ScoringPlay = play.ScoringPlay,
                        TeamId = play.TeamId,
                        AwayScore = play.AwayScore,
                        HomeScore = play.HomeScore,
                        GroupId = play.GroupId
                    }).ToList()
            };
        }
    }
}