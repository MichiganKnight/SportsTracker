namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
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