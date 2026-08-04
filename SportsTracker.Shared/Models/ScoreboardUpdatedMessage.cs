using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models
{
    public sealed class ScoreboardUpdatedMessage
    {
        public required League League { get; init; }
        
        public DateTime UpdatedUtc { get; init; }
    }
}