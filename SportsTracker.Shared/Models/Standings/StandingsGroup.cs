namespace SportsTracker.Shared.Models.Standings
{
    public sealed class StandingsGroup
    {
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public IReadOnlyList<TeamStanding> Teams { get; init; } = [];
    }
}