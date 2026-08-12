namespace SportsTracker.Backend.Integrations.ESPN.DTOs.BoxScore
{
    public sealed class BoxScoreStatTableDto
    {
        public string? Type { get; init; }
        
        public List<string>? Names { get; init; } = [];
        public List<string>? Keys { get; init; } = [];
        public List<string>? Labels { get; init; } = [];
        public List<string>? Descriptions { get; init; } = [];
        public List<string>? Totals { get; init; } = [];
        
        public List<BoxScoreAthleteEntryDto>? Athletes { get; init; } = [];
    }
}