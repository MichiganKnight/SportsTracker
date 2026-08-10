namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Common
{
    public sealed class AthleteDto
    {
        public string Id { get; init; } = string.Empty;
        
        public string FullName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        public string? Jersey { get; init; }
        
        //public PositionDto? Position { get; init; }
        public AthleteTeamDto Team { get; init; }
    }

    /*public sealed class PositionDto
    {
        public string Abbreviation { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }*/
    
    public sealed class AthleteTeamDto
    {
        public string Id { get; init; } = string.Empty;
    }
}