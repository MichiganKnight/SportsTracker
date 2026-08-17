namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class AddressDto
    {
        public string? City { get; init; }
        public string? State { get; init; }
    }
    
    public sealed class AthleteDto
    {
        public string? Id { get; init; }
        
        public string? FullName { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        
        public string? Headshot { get; init; }
        public string? Jersey { get; init; }
        
        public AthleteTeamDto? Team { get; init; }
    }
    
    public sealed class AthleteTeamDto
    {
        public string Id { get; init; } = string.Empty;
    }
    
    public sealed class EspnLogoDto
    {
        public string? Href { get; init; }
        
        public int Width { get; init; }
        public int Height { get; init; }
        
        public string? Alt { get; init; }
        
        public List<string> Rel { get; init; } = [];
    }
    
    public sealed class RecordDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? Type { get; init; }

        public string Summary { get; init; } = string.Empty;
        public string? DisplayValue { get; init; }
    }
    
    public sealed class StatusDto
    {
        public StatusTypeDto Type { get; init; } = new();
        
        public int Period { get; init; }
        
        public string DisplayClock { get; init; } = string.Empty;
        
        public List<FeaturedAthleteDto>? FeaturedAthletes { get; init; }
    }

    public sealed class StatusTypeDto
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        
        public bool Completed { get; init; }
        
        public string Description { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;
        public string ShortDetail { get; init; } = string.Empty;
    }
    
    public sealed class TeamDto
    {
        public string Id { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Nickname { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        
        public string? AlternateColor { get; init; }
        
        public string? Logo { get; init; }
        public List<EspnLogoDto> Logos { get; init; } = [];
    }
    
    public sealed class TeamReferenceDto
    {
        public string? Id { get; init; }
    }
    
    public sealed class VenueDto
    {
        public string Id { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        
        public AddressDto? Address { get; init; }
        
        public bool? Indoor { get; init; }
    }
}