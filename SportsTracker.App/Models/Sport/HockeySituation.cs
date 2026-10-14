namespace SportsTracker.App.Models.Sport
{
    public sealed class HockeySituation
    {
        public int? Period { get; init; }
        
        public string? Clock { get; init; }
        
        public string? HomeTeamId { get; init; }
        public string? AwayTeamId { get; init; }
        
        public string? PowerPlayTeamId { get; init; }
        public string? Strength { get; init; }
        
        public string? LastPlay { get; init; }
        public string? LastPlayType { get; init; }
        public string? LastPlayTeamId { get; init; }
        public string? LastPlayPlayer { get; init; }
    }
}