using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.AthleteInfo
{
    public sealed class AthleteDetails
    {
        public string Id { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }

        public bool IsActive { get; init; }
        
        public string? Status { get; init; }
        
        public int? Age { get; init; }
        
        public string? DateOfBirth { get; init; }
        public string? BirthPlace { get; init; }
        
        public string? Height { get; init; }
        public string? Weight { get; init; }
        
        public int? DebutYear { get; init; }
        
        /*
         * Teal-Sport Fields
         */
        
        public AthleteTeam? Team { get; init; }
        
        public AthletePosition? Position { get; init; }
        
        public string? Jersey { get; init; }
        public string? College { get; init; }
        public string? Experience { get; init; }
        public string? Draft { get; init; }
        public string? BatsThrows { get; init; }
        
        /*
         * Golf Specific Fields
         */
        
        public int? TurnedPro { get; init; }
        
        public string? Hand { get; init; }
        
        public string? Citizenship { get; init; }
        public string? CountryFlag { get; init; }
        
        /*
         * Common Quick Stat Block
         */
        
        public string? StatsSummaryTitle { get; init; }
        
        public IReadOnlyList<AthleteStatSummary> StatsSummary { get; init; } = [];
    }

    public sealed class AthleteTeam
    {
        public string Id { get; init; } = string.Empty;
        
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? DarkLogo { get; init; }
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
    }

    public sealed class AthletePosition
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
    }

    public sealed class AthleteStatSummary
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public double? Value { get; init; }
        public string? DisplayValue { get; init; }
        
        public int? Rank { get; init; }
        public string? RankDisplayValue { get; init; }
    }
}