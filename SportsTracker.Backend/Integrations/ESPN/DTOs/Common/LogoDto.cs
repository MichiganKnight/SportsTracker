namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class LogoDto
    {
        public string Href { get; init; } = string.Empty;
        
        public int Width { get; init; }
        public int Height { get; init; }
        
        public string? Alt { get; init; }
    }
}