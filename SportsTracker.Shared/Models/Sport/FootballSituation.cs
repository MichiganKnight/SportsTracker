namespace SportsTracker.Shared.Models.Sport
{
    public sealed class FootballSituation
    {
        public int Quarter { get; init; }
        public string? Clock { get; init; }
        
        public int? Down { get; init; }
        public int? Distance { get; init; }
        
        public string? PossessionTeamId { get; init; }
        
        public string? LastPlay { get; init; }
    }
}