using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.GameInfo;

namespace SportsTracker.Shared.Models
{
    public sealed class CachedScoreboard
    {
        public League League { get; init; }

        public IReadOnlyList<Game> Games { get; init; } = [];
        
        public string? LeagueLogo { get; init; }
        public string? LeagueDarkLogo { get; init; }

        public DateTime LastUpdatedUtc { get; init; }
    }
}