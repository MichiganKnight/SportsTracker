namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class AthleteDto
    {
        public string? Id { get; init; }
        
        public string? FullName { get; init; }
        public string? DisplayName { get; init; }
        public string? ShortName { get; init; }
        
        public string? Headshot { get; init; }
        public string? Jersey { get; init; }
        
        public AthleteTeamDto? Team { get; init; }
    }
    
    public sealed class AthleteTeamDto
    {
        public string Id { get; init; } = string.Empty;
    }
}