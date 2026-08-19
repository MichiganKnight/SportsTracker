namespace SportsTracker.App.Integrations.ESPN.DTOs.Athlete
{
    public sealed class AthleteProfileResponseDto
    {
        public AthleteProfileDto? Athlete { get; init; }
    }

    public sealed class AthleteProfileDto
    {
        public string? Id { get; init; }
        
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        
        public string? DisplayName { get; init; }
        public string? FullName { get; init; }
        
        public int? DebutYear { get; init; }
        public int? TurnedPro { get; init; }
        
        public string? Jersey { get; init; }
        
        public AthleteHeadshotDto? Headshot { get; init; }
        public AthletePositionDto? Position { get; init; }
        public AthleteProfileTeamDto? Team { get; init; }
        
        public bool? Active { get; init; }
        
        public AthleteStatusDto? Status { get; init; }
        public AthleteCollegeDto? College { get; init; }
        public AthleteStatsSummaryDto? StatsSummary { get; init; }
        
        public string? DisplayBirthPlace { get; init; }
        
        public string? DisplayHeight { get; init; }
        public string? DisplayWeight { get; init; }
        
        public string? DisplayDOB { get; init; }
        
        public int? Age { get; init; }
        
        public string? DisplayJersey { get; init; }
        public string? DisplayExperience { get; init; }
        public string? DisplayDraft { get; init; }
        public string? DisplayBatsThrows { get; init; }
        
        /*
         * Golf
         */
        
        public AthleteHandDto? Hand { get; init; }
        
        public string? Citizenship { get; init; }
        
        public AthleteFlagDto? Flag { get; init; }
    }
    
    public sealed class AthleteHeadshotDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }

    public sealed class AthletePositionDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
    }
    
    public sealed class AthleteStatusDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? Type { get; init; }
        public string? Abbreviation { get; init; }
    }

    public sealed class AthleteCollegeDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? ShortName { get; init; }
        public string? Abbrev { get; init; }
    }

    public sealed class AthleteHandDto
    {
        public string? Type { get; init; }
        public string? Abbreviation { get; init; }
        public string? DisplayValue { get; init; }
    }

    public sealed class AthleteFlagDto
    {
        public string? Href { get; init; }
        public string? Alt { get; init; }
    }

    public sealed class AthleteProfileTeamDto
    {
        public string? Id { get; init; }
        
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
        
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
        
        public List<EspnLogoDto>? Logos { get; init; } = [];
    }

    public sealed class AthleteStatsSummaryDto
    {
        public string? DisplayName { get; init; }
        
        public List<AthleteSummaryStatDto> Statistics { get; init; } = [];
    }
    
    public sealed class AthleteSummaryStatDto
    {
        public string? Name { get; init; }
        
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Description { get; init; }
        public string? Abbreviation { get; init; }
        
        public double? Value { get; init; }
        public string? DisplayValue { get; init; }
        
        public int? Rank { get; init; }
        public string? RankDisplayValue { get; init; }
    }
}