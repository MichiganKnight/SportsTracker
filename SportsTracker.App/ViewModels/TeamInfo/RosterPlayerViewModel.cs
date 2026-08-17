namespace SportsTracker.App.ViewModels.TeamInfo
{
    public sealed class RosterPlayerViewModel
    {
        public string Id { get; init; } = string.Empty;
        
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Jersey { get; init; }
        public string? Headshot { get; init; }
        public string? Position { get; init; }
        public string? PositionAbbreviation { get; init; }
        
        public int? Age { get; init; }
        
        public string? Height { get; init; }
        public string? Weight { get; init; }
        
        public int? ExperienceYears { get; init; }
        
        public string? Status { get; init; }
        public string? Bats { get; init; }
        public string? Throws { get; init; }
        
        public string? BirthPlace { get; init; }
    }
}