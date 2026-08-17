using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.BoxScore
{
    public sealed class BoxScoreViewModel
    {
        public string GameId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<TeamBoxScoreViewModel> Teams { get; init; } = [];
    }
}