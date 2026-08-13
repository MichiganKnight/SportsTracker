using SportsTracker.Frontend.ViewModels.GameDetails;

namespace SportsTracker.Frontend.ViewModels.GameInfo
{
    public sealed class GamePageViewModel<TContent>
    {
        public GameDetailsViewModel Game { get; init; } = null!;

        public TContent Content { get; init; } = default!;
    }
}