namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
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
        
        public List<TeamLogoDto> Logos { get; init; } = [];
        
        public TeamRecordContainerDto? Record { get; init; }
        public TeamGroupDto? Groups { get; init; }
        public TeamFranchiseDto? Franchise { get; init; }
    }
}