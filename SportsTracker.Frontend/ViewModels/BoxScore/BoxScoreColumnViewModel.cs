namespace SportsTracker.Frontend.ViewModels.BoxScore
{
    public sealed class BoxScoreColumnViewModel
    {
        public string Key { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        
        public string? Description { get; init; }
    }
}