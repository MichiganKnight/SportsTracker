namespace SportsTracker.Shared.Models
{
    public sealed class CachedScoreboard
    {
        public IReadOnlyList<Game> Games { get; init; } = [];
        
        public DateTime LastUpdatedUtc { get; init; }
        
        public TimeSpan? RefreshDuration { get; init; }
    }
}