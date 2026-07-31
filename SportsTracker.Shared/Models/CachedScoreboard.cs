using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models
{
    public sealed class CachedScoreboard
    {
        public League League { get; init; }

        public IReadOnlyList<Game> Games { get; init; } = [];

        public DateTime LastUpdatedUtc { get; init; }
    }
}