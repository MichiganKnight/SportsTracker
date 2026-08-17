namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class BoxScoreAthleteDto
    {
        public string? Id { get; init; }
        public string? FullName { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        public string? Jersey { get; init; }
        
        public BoxScoreHeadshotDto? Headshot { get; init; }
    }
    
    public sealed class BoxScoreHeadshotDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }
    
    public sealed class BoxScoreAthleteEntryDto
    {
        public bool? Active { get; init; }
        
        public BoxScoreAthleteDto? Athlete { get; init; }
        
        public bool? Starter { get; init; }
        
        public int? BatOrder { get; init; }
        
        public BoxScorePositionDto? Position { get; init; }
        
        public List<BoxScoreNoteDto>? Notes { get; init; }
        
        public List<string>? Stats { get; init; } = [];
    }
    
    public sealed class BoxScoreDto
    {
        public List<BoxScorePlayerTeamDto>? Players { get; init; }
    }
    
    public sealed class BoxScoreNoteDto
    {
        public string? Type { get; init; }
        public string? Text { get; init; }
    }
    
    public sealed class BoxScorePlayerTeamDto
    {
        public BoxScoreTeamDto? Team { get; init; }
        
        public List<BoxScoreStatTableDto>? Statistics { get; init; } = [];
        
        public int? DisplayOrder { get; init; }
    }
    
    public sealed class BoxScorePositionDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
    }
    
    public sealed class BoxScoreStatTableDto
    {
        public string? Type { get; init; }
        public string? Name { get; init; }
        public string? Text { get; init; }
        
        public List<string>? Names { get; init; } = [];
        public List<string>? Keys { get; init; } = [];
        public List<string>? Labels { get; init; } = [];
        public List<string>? Descriptions { get; init; } = [];
        public List<string>? Totals { get; init; } = [];
        
        public List<BoxScoreAthleteEntryDto>? Athletes { get; init; } = [];
    }
    
    public sealed class BoxScoreTeamDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        public string? Logo { get; init; }
    }
}