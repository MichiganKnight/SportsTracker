using SportsTracker.App.ViewModels.GameDetails;

namespace SportsTracker.App.ViewModels.GameInfo
{
    public sealed class GamePageViewModel<TContent>
    {
        public GameDetailsViewModel Game { get; init; } = null!;

        public TContent Content { get; init; } = default!;
    }
}