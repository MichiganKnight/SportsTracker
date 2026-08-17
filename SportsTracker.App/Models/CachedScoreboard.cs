using SportsTracker.App.Enums;
using SportsTracker.App.Models.GameInfo;

namespace SportsTracker.App.Models
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