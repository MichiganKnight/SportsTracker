using SportsTracker.Frontend.ViewModels.GameDetails;

namespace SportsTracker.Frontend.ViewModels.PlayByPlay
{
    public sealed class PlayByPlayPageViewModel
    {
        public GameDetailsViewModel Game { get; init; } = null!;

        public PlayByPlayViewModel PlayByPlay { get; init; } = null!;
    }
}