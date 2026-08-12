namespace SportsTracker.Shared.Models.BoxScore
{
    public sealed class BoxScoreColumn
    {
        public string Key { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        
        public string? Description { get; init; }
    }
}