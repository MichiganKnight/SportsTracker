namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Groups
{
    public sealed class GroupDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<GroupDto>? Children { get; init; }
        public List<GroupTeamDto>? Teams { get; init; }
    }
}