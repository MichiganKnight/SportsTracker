namespace SportsTracker.Shared.Models.GameDetails
{
    public sealed class GameDetailsTeam
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public int Score { get; init; }
        
        public bool Winner { get; init; }
        
        public string? Record { get; init; }
        
        public int? Hits { get; init; }
        public int? Errors { get; init; }
        
        public IReadOnlyList<LineScore> LineScores { get; init; } = [];
    }
}