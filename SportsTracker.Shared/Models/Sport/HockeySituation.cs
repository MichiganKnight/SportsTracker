namespace SportsTracker.Shared.Models.Sport
{
    public sealed class HockeySituation
    {
        public int Period { get; init; }
        public string? Clock { get; init; }
        
        public string? PossessionTeamId { get; init; }
        
        public string? LastPlay { get; init; }   
    }
}