namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
    public sealed class BoxScorePositionDto
    {
        public string? Id { get; init; }
        
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? Abbreviation { get; init; }
    }
}