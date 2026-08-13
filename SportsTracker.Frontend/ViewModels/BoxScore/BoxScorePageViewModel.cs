using SportsTracker.Frontend.ViewModels.GameDetails;

namespace SportsTracker.Frontend.ViewModels.BoxScore
{
    public sealed class BoxScorePageViewModel
    {
        public GameDetailsViewModel Game { get; init; } = null!;

        public BoxScoreViewModel BoxScore { get; init; } = null!;
    }
}