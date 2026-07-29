namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class VenueDto
    {
        public string Id { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        
        public AddressDto? Address { get; init; }
        
        public bool Indoor { get; init; }
    }
}