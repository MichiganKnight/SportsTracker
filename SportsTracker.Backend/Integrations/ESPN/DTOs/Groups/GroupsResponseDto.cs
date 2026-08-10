namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Groups
{
    public sealed class GroupsResponseDto
    {
        public string? Status { get; init; }
        
        public List<GroupDto?>? Groups { get; init; }
    }
}