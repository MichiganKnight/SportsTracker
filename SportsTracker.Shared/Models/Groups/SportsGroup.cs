namespace SportsTracker.Shared.Models.Groups
{
    public sealed class SportsGroup
    {
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public IReadOnlyList<string> TeamIds { get; init; } = [];
        public IReadOnlyList<SportsGroup> Children { get; init; } = [];
    }
}