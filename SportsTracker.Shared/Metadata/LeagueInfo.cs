using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Metadata
{
    public sealed class LeagueInfo
    {
        public League League { get; init; }
        public Sport Sport { get; init; }
        
        public string EspnSport { get; init; } = string.Empty;
        public string EspnLeague { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Icon { get; init; }
        public string? Logo { get; init; }
        public string? DarkLogo { get; init; }
        
        public int DisplayOrder { get; init; }
    }
}