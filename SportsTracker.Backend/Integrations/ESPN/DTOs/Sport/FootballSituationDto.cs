namespace SportsTracker.Backend.Integrations.ESPN.DTOs.Sport
{
    public sealed class FootballSituationDto
    {
        public int? Down { get; init; }
        public int? Distance { get; init; }
        
        public string? DownDistanceText { get; init; }
        public string? PossessionTeamId { get; init; }
        public string? PossessionText { get; init; }
    }
}