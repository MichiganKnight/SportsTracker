namespace SportsTracker.Shared.Models.TeamInfo
{
    public sealed class RosterGroup
    {
        public string Name { get; init; } = string.Empty;
        
        public IReadOnlyList<RosterPlayer> Players { get; init; } = [];
    }
}