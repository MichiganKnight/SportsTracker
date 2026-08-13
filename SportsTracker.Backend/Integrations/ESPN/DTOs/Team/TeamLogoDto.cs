namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Team
{
    public sealed class TeamLogoDto
    {
        public string? Href { get; init; }
        
        public int? Width { get; init; }
        public int? Height { get; init; }
        
        public List<string> Rel { get; init; } = [];
    }
}