using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Models.Sport;
using SportsTracker.App.ViewModels.Golf;

namespace SportsTracker.App.Mapping
{
    public interface IGolfEventCardViewModelMapper
    {
        GolfEventCardViewModel Map(Game game);
    }
    
    public sealed class GolfEventCardViewModelMapper : IGolfEventCardViewModelMapper
    {
        private const int LeaderCount = 5;

        public GolfEventCardViewModel Map(Game game)
        {
            GolfTournament golf = game.Golf ?? throw new ArgumentException("Game Does not Contain Golf Data", nameof(game));

            return new GolfEventCardViewModel
            {
                EventId = game.Id,

                Name = golf.Name,

                StartTime = game.StartTime,
                EndTime = golf.EndTime,

                Status = game.StatusText,

                IsLive = game.IsLive,
                IsFinal = game.IsFinal,
                IsUpcoming = game.IsUpcoming,

                Leaders = golf.Leaderboard
                    .OrderBy(player => player.Position ?? int.MaxValue)
                    .Take(LeaderCount)
                    .Select(player => new GolfLeaderboardRowViewModel
                    {
                        AthleteId = player.AthleteId,
                        Name = player.Name,
                        CountryFlag = player.CountryFlag,
                        Country = player.Country,
                        Position = player.Position,
                        Score = player.ScoreToPar
                    })
                    .ToList()
            };
        }
    }
}