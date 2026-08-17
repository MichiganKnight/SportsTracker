namespace SportsTracker.App.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamDetailsResponseDto
    {
        public TeamDetailsDto? Team { get; init; }
    }
    
    public sealed class TeamDetailsDto
    {
        public string? Id { get; init; }
        
        public string? Slug { get; init; }
        public string? Location { get; init; }
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public bool? IsActive { get; init; }
        
        public List<EspnLogoDto> Logos { get; init; } = [];
        
        public TeamRecordContainerDto? Record { get; init; }
        public TeamGroupDto? Groups { get; init; }
        public TeamFranchiseDto? Franchise { get; init; }
    }
    
    public sealed class TeamFranchiseDto
    {
        public string? Id { get; init; }
        
        public TeamVenueDto? Venue { get; init; }
    }
    
    public sealed class TeamGroupDto
    {
        public string? Id { get; init; }
        
        public TeamGroupParentDto? Parent { get; init; }
        
        public bool? IsConference { get; init; }
    }
    
    public sealed class TeamGroupParentDto
    {
        public string? Id { get; init; }
    }
    
    public sealed class TeamRecordContainerDto
    {
        public List<TeamRecordItemDto> Items { get; init; } = [];
    }
    
    public sealed class TeamRecordItemDto
    {
        public string? Description { get; init; }
        public string? Type { get; init; }
        public string? Summary { get; init; }
    }
    
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
    
    public sealed class TeamVenueAddressDto
    {
        public string? City { get; init; }
        public string? State { get; init; }
        public string? ZipCode { get; init; }
    }
    
    public sealed class TeamVenueImageDto
    {
        public string? Href { get; init; }
        
        public int? Width { get; init; }
        public int? Height { get; init; }
        
        public List<string> Rel { get; init; } = [];
    }
}