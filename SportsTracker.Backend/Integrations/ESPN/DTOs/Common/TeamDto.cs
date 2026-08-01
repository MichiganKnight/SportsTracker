namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class TeamDto
    {
        public string Id { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Nickname { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        
        public string? AlternateColor { get; init; }
        
        public string? Logo { get; init; }
        public List<LogoDto> Logos { get; init; } = [];
    }
}