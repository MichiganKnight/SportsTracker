namespace SportsTracker.App.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamsResponseDto
    {
        public List<TeamsSportDto> Sports { get; init; } = [];
    }

    public sealed class TeamsSportDto
    {
        public List<TeamsLeagueDto> Leagues { get; init; } = [];
    }

    public sealed class TeamsLeagueDto
    {
        public List<TeamsEntryDto> Teams { get; init; } = [];
    }

    public sealed class TeamsEntryDto
    {
        public TeamDetailsDto? Team { get; init; }
    }
}