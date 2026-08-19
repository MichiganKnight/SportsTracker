using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.AthleteInfo
{
    public sealed class AthleteDetailsViewModel
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        
        public bool IsActive { get; init; }
        
        public string? Status { get; init; }
        
        public string? TeamId { get; init; }
        public string? TeamName { get; init; }
        
        public string? TeamLogo { get; init; }
        public string? TeamDarkLogo { get; init; }
        
        public string? Position { get; init; }
        
        public string? Jersey { get; init; }
        
        public int? Age { get; init; }
        
        public string? DateOfBirth { get; init; }
        public string? BirthPlace { get; init; }
        
        public string? Height { get; init; }
        public string? Weight { get; init; }
        
        public string? College { get; init; }
        public string? Experience { get; init; }
        public string? Draft { get; init; }
        public string? BatsThrows { get; init; }
        
        public int? TurnedPro { get; init; }
        
        public string? Hand { get; init; }
        
        public string? Citizenship { get; init; }
        public string? CountryFlag { get; init; }
        
        public string? StatsSummaryTitle { get; init; }
        
        public IReadOnlyList<AthleteStatSummaryViewModel> StatsSummary { get; init; } = [];

        public bool HasTeam => !string.IsNullOrWhiteSpace(TeamName);
        public bool HasStatsSummary => StatsSummary.Count > 0;
        public bool IsGolf => League == League.PGA;
    }

    public sealed class AthleteStatSummaryViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        
        public string? Rank { get; init; }
    }
}