namespace SportsTracker.Shared.Models
{
    public sealed class ScoreboardUpdatedMessage
    {
        public string League { get; init; } = string.Empty;
        
        public DateTime UpdatedUtc { get; init; }
    }
}