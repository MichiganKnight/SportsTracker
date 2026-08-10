using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.Standings
{
    public sealed class StandingsGroup
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public StandingsGroupType Type { get; init; }
        
        public IReadOnlyList<TeamStanding> Teams { get; init; } = [];
    }
}