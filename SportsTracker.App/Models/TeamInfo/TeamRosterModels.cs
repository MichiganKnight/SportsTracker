using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.TeamInfo
{
    public sealed class TeamRoster
    {
        public string TeamId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public int? Season { get; init; }
        
        public string? SeasonName { get; init; }
        
        public IReadOnlyList<RosterGroup> Groups { get; init; } = [];
    }
    
    public sealed class RosterGroup
    {
        public string Name { get; init; } = string.Empty;
        
        public IReadOnlyList<RosterPlayer> Players { get; init; } = [];
    }
    
    public sealed class RosterPlayer
    {
        public string Id { get; init; } = string.Empty;
        
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? Jersey { get; init; }
        public string? Headshot { get; init; }
        public string? Position { get; init; }
        public string? PositionAbbreviation { get; init; }
        
        public int? Age { get; init; }
        
        public DateTime? DateOfBirth { get; init; }
        
        public string? Height { get; init; }
        public string? Weight { get; init; }
        
        public int? ExperienceYears { get; init; }
        
        public string? Status { get; init; }
        public string? Bats { get; init; }
        public string? Throws { get; init; }
        
        public string? BirthPlace { get; init; }
    }
}