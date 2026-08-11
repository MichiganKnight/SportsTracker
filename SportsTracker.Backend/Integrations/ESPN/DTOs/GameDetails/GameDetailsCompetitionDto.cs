using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class GameDetailsCompetitionDto
    {
        public string? Id { get; init; }
        
        public DateTime? Date { get; init; }
        
        public int? Attendance { get; init; }
        
        public bool? NeutralSite { get; init; }
        public bool? PlayByPlayAvailable { get; init; }
        
        public VenueDto? Venue { get; init; }
        
        public List<GameDetailsCompetitorDto>? Competitors { get; init; } = [];
        
        public StatusDto? Status { get; init; }
        
        public List<BroadcastDto>? Broadcasts { get; init; } = [];
        
        public string? Broadcast { get; init; }
        
        public List<HeadlineDto>? Headlines { get; init; } = [];
    }
}