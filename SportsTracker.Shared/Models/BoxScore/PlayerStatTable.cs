namespace SportsTracker.Shared.Models.BoxScore
{
    public sealed class PlayerStatTable
    {
        public string Type { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<BoxScoreColumn> Columns { get; init; } = [];
        public IReadOnlyList<PlayerStatRow> Players { get; init; } = [];
        
        public IReadOnlyList<string> Totals { get; init; } = []; 
    }
}