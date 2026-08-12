namespace SportsTracker.Frontend.ViewModels.BoxScore
{
    public sealed class PlayerStatTableViewModel
    {
        public string Type { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;

        public IReadOnlyList<BoxScoreColumnViewModel> Columns { get; init; } = [];
        public IReadOnlyList<PlayerStatRowViewModel> Players { get; init; } = [];
        
        public IReadOnlyList<string> Totals { get; init; } = [];
    }
}