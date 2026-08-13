namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamRecordContainerDto
    {
        public List<TeamRecordItemDto> Items { get; init; } = [];
    }
}