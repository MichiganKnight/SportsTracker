namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Groups
{
    public sealed class GroupTeamDto
    {
        public string? Id { get; init; }
        
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
    }
}