namespace SportsTracker.Frontend.ViewModels.Standings
{
    public sealed class TeamStandingViewModel
    {
        public string TeamId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        
        public int Wins { get; init; }
        public int Losses { get; init; }
        
        public double WinPercentage { get; init; }
        public double? GamesBack { get; init; }
        
        public int? RunsScored { get; init; }
        public int? RunsAllowed { get; init; }
        public int? RunDifferential { get; init; }
        
        public string? Streak { get; init; }
        
        public int? PlayoffSeed { get; init; }
        
        public string WinPercentageDisplay => WinPercentage.ToString(".000");
        public string GamesBackDisplay => GamesBack is null or 0 ? "-" : GamesBack.Value.ToString("0.#");
    }
}