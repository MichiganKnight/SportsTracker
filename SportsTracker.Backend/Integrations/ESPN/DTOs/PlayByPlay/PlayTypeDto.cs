namespace SportsTracker.Backend.Integrations.ESPN.DTOs.PlayByPlay
{
    public sealed class PlayTypeDto
    {
        public string? Id { get; init; }
        
        public string? Type { get; init; }
        public string? Text { get; init; }
        public string? Abbreviation { get; init; }
        public string? AlternativeText { get; init; }
    }
}