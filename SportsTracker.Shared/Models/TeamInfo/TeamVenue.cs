namespace SportsTracker.Shared.Models.TeamInfo
{
    public sealed class TeamVenue
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        
        public string? City { get; init; }
        public string? State { get; init; }
        public string? ZipCode { get; init; }
        
        public bool? Grass { get; init; }
        public bool? Indoor { get; init; }
        
        public string? Image { get; init; }
    }
}