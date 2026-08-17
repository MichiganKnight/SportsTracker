using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.TeamInfo
{
    public sealed class TeamDetails
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? DarkLogo { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public bool IsActive { get; init; }
        
        public string? GroupId { get; init; }
        
        public IReadOnlyList<TeamRecord> Records { get; init; } = [];
        
        public TeamVenue? Venue { get; init; }
    }
    
    public sealed class TeamRecord
    {
        public string Type { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
    }
    
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