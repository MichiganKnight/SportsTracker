using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class FeaturedAthleteDto
    {
        public string? Name { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortDisplayName { get; init; }
        public string? Abbreviation { get; init; }
        
        public long? PlayerId { get; init; }
        
        public AthleteDto? Athlete { get; init; }
        public TeamReferenceDto? Team { get; init; }
        
        public List<GameDetailsStatDto>? Statistics { get; init; } = [];
    }
}