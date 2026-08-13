namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamGroupDto
    {
        public string? Id { get; init; }
        
        public TeamGroupParentDto? Parent { get; init; }
        
        public bool? IsConference { get; init; }
    }
}