using SportsTracker.App.Models.GameInfo;
using SportsTracker.App.Models.Sport;
using SportsTracker.App.ViewModels.Golf;

namespace SportsTracker.App.Mapping
{
    public interface IGolfTournamentViewModelMapper
    {
        GolfTournamentViewModel Map(Game game);
    }
    
    public class GolfTournamentViewModelMapper : IGolfTournamentViewModelMapper
    {
        public GolfTournamentViewModel Map(Game game)
        {
            GolfTournament? golf = game.Golf ?? throw new ArgumentException("Game Does Not Contain Golf Data", nameof(game));

            return new GolfTournamentViewModel()
            {
                EventId = game.Id,
                Name = golf.Name,

                StartTime = game.StartTime,
                EndTime = golf.EndTime,

                Status = game.StatusText,

                IsLive = game.IsLive,
                IsFinal = game.IsFinal,
                IsUpcoming = game.IsUpcoming,
                
                Venue = golf.Venue,
                Location = golf.Location,
                
                Leaderboard = golf.Leaderboard
                    .OrderBy(entry => entry.Position ?? int.MaxValue)
                    .Select(entry => new GolfLeaderboardRowViewModel
                    {
                        AthleteId = entry.AthleteId,

                        Name = entry.Name,

                        CountryFlag = entry.CountryFlag,
                        Country = entry.Country,

                        Position = entry.Position,

                        Score = entry.ScoreToPar,

                        Rounds = entry.Rounds
                            .OrderBy(round => round.Round)
                            .Select(round => new GolfRoundViewModel
                            {
                                Round = round.Round,
                                Strokes = round.Strokes,
                                ScoreToPar = round.ScoreToPar,
                                
                                Holes = round.Holes
                                    .OrderBy(hole => hole.Hole)
                                    .Select(hole => new GolfHoleViewModel
                                    {
                                        Hole = hole.Hole,
                                        Strokes = hole.Strokes,
                                        ScoreToPar = hole.ScoreToPar
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}