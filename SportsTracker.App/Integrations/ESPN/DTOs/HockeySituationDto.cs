namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class HockeySituationDto
    {
        public HockeyLastPlayDto? LastPlay { get; init; }
    }

    public sealed class HockeyLastPlayDto
    {
        public string? Id { get; init; }
        
        public HockeyPlayTypeDto? Type { get; init; }
        
        public string? Text { get; init; }
        
        public int? ScoreValue { get; init; }
        
        public TeamReferenceDto? Team { get; init; }
        
        public List<AthleteDto>? AthletesInvolved { get; init; }
    }

    public sealed class HockeyPlayTypeDto
    {
        public string? Id { get; init; }
        
        public string? Text { get; init; }
        public string? Abbreviation { get; init; }
    }
}