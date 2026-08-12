namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
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
}