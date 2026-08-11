using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class GameDetailsResponseDto
    {
        public string? Id { get; init; }
        public string? Uid { get; init; }
        
        public DateTime? Date { get; init; }
        
        public string? Name { get; init; }
        public string? ShortName { get; init; }
        
        public List<GameDetailsCompetitionDto>? Competitions { get; init; } = [];
        
        public StatusDto? Status { get; init; }
    }
}