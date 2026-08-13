namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamVenueDto
    {
        public string? Id { get; init; }
        
        public string? FullName { get; init; }
        public string? ShortName { get; init; }
        
        public TeamVenueAddressDto Address { get; init; }
        
        public bool? Grass { get; init; }
        public bool? Indoor { get; init; }
        
        public List<TeamVenueImageDto> Images { get; init; } = [];
    }
}