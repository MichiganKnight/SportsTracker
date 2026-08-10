using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models.Standings
{
    public sealed class LeagueStandings
    {
        public League League { get; init; }
        
        public int Season { get; init; }
        
        public IReadOnlyList<StandingsGroup> Groups { get; init; } = [];
    }
}