namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Scoreboard
{
    public sealed class VenueDto
    {
        public string Id { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        
        public bool Indoor { get; init; }
    }
}