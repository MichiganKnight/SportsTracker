namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamFranchiseDto
    {
        public string? Id { get; init; }
        
        public TeamVenueDto? Venue { get; init; }
    }
}