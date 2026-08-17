namespace SportsTracker.App.ViewModels.GameDetails
{
    public sealed class BaseballLineScoreRowViewModel
    {
        public GameDetailsTeamViewModel Team { get; init; } = null!;
        
        public int MaxInning { get; init; }
        
        public bool IsHomeTeam { get; init; }
    }
}