namespace SportsTracker.App.Integrations.ESPN.DTOs
{
    public sealed class GroupDto
    {
        public string? Name { get; init; }
        public string? Abbreviation { get; init; }
        
        public List<GroupDto>? Children { get; init; }
        public List<GroupTeamDto>? Teams { get; init; }
    }
    
    public sealed class GroupTeamDto
    {
        public string? Id { get; init; }
        
        public string? Abbreviation { get; init; }
        public string? DisplayName { get; init; }
    }
    
    public sealed class GroupsResponseDto
    {
        public string? Status { get; init; }
        
        public List<GroupDto?>? Groups { get; init; }
    }
}