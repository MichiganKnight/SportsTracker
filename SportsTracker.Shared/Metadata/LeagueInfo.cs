using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Metadata
{
    public sealed class LeagueInfo
    {
        public League League { get; init; }
        public Sport Sport { get; init; }
        
        public string DisplayName { get; init; } = string.Empty;
        public string EspnSport { get; init; } = string.Empty;
        public string EspnLeague { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;
        
        public int DisplayOrder { get; init; }
    }
}