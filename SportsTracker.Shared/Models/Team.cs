using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Models
{
    public class Team
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public string Name { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string Nickname { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        
        public string? AlternateColor { get; init; }
        public Logo? Logo { get; init; }
        public Record? Record { get; init; }
    }
}