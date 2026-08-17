namespace SportsTracker.App.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamRosterResponseDto
    {
        public TeamRosterSeasonDto? Season { get; init; }
        
        public List<TeamRosterGroupDto> Athletes { get; init; } = [];
    }
    
    public sealed class TeamRosterSeasonDto
    {
        public int? Year { get; init; }
        
        public string? DisplayName { get; init; }
        
        public int? Type { get; init; }
        
        public string? Name { get; init; }
    }
    
    public sealed class TeamRosterGroupDto
    {
        public string? Position { get; init; }
        
        public List<RosterAthleteDto> Items { get; init; } = [];
    }
    
    public sealed class RosterAthleteDto
    {
        public string? Id { get; init; }
        
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? FullName { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        
        public double? Weight { get; init; }
        public string? DisplayWeight { get; init; }
        
        public double? Height { get; init; }
        public string? DisplayHeight { get; init; }
        
        public int? Age { get; init; }
        
        public DateTime? BirthDate { get; init; }
        
        public int? DebutYear { get; init; }
        
        public string? Slug { get; init; }
        public string? Jersey { get; init; }
        
        public RosterHeadshotDto? Headshot { get; init; }
        public RosterPositionDto? Position { get; init; }
        public RosterExperienceDto? Experience { get; init; }
        public RosterStatusDto? Status { get; init; }
        public RosterHandDto? Bats { get; init; }
        public RosterHandDto? Throws { get; init; }
        public RosterBirthPlaceDto? BirthPlace { get; init; }
    }
    
    public sealed class RosterHeadshotDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }
    
    public sealed class RosterPositionDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public bool? Leaf { get; init; }
        
        public RosterPositionParentDto? Parent { get; init; }
    }

    public sealed class RosterPositionParentDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
    }

    public sealed class RosterExperienceDto
    {
        public int? Years { get; init; }
    }

    public sealed class RosterStatusDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Type { get; init; }
        public string? Abbreviation { get; init; }
    }
    
    public sealed class RosterHandDto
    {
        public string? Type { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayValue { get; init; }
    }
    
    public sealed class RosterBirthPlaceDto
    {
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Country { get; init; }
        public string? DisplayText { get; init; }
    }
}