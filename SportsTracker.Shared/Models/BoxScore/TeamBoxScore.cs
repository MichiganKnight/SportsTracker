namespace SportsTracker.Shared.Models.BoxScore
{
    public sealed class TeamBoxScore
    {
        public string TeamId { get; init; } = string.Empty;
        public string TeamName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        
        public IReadOnlyList<PlayerStatTable> Tables { get; init; } = [];
    }
}