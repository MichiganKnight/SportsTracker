using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.TeamInfo
{
    public sealed class TeamRoster
    {
        public string TeamId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public int? Season { get; init; }
        
        public string? SeasonName { get; init; }
        
        public IReadOnlyList<RosterGroup> Groups { get; init; } = [];
    }
}