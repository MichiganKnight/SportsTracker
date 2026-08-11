using SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class StatusDto
    {
        public StatusTypeDto Type { get; init; } = new();
        
        public int Period { get; init; }
        
        public string DisplayClock { get; init; } = string.Empty;
        
        public List<FeaturedAthleteDto>? FeaturedAthletes { get; init; }
    }

    public sealed class StatusTypeDto
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        
        public bool Completed { get; init; }
        
        public string Description { get; init; } = string.Empty;
        public string Detail { get; init; } = string.Empty;
        public string ShortDetail { get; init; } = string.Empty;
    }
}