namespace SportsTracker.App.ViewModels.Standings
{
    public sealed class TeamStandingViewModel
    {
        public string TeamId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        
        public int Wins { get; init; }
        public int Losses { get; init; }
        public int? Ties { get; init; }
        
        public double WinPercentage { get; init; }
        public double? GamesBack { get; init; }
        
        public int? PointsFor { get; init; }
        public int? PointsAgainst { get; init; }
        public int? PointDifferential { get; init; }
        
        public string? Streak { get; init; }
        
        public int? PlayoffSeed { get; init; }
        
        public string WinPercentageDisplay => WinPercentage.ToString(".000");
        public string GamesBackDisplay => GamesBack is null or 0 ? "-" : GamesBack.Value.ToString("0.#");
    }
}