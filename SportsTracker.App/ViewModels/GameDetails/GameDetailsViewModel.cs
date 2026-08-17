using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.GameDetails
{
    public sealed class GameDetailsViewModel
    {
        public string GameId { get; init; } =  string.Empty;
        
        public League League { get; init; }
        
        public string LeagueName { get; init; } = string.Empty;
        
        public DateTime StartTime { get; init; }
        
        public string Status { get; init; } = string.Empty;
        
        public bool IsLive { get; init; }
        public bool IsFinal { get; init; }

        public GameDetailsTeamViewModel AwayTeam { get; init; } = null!;
        public GameDetailsTeamViewModel HomeTeam { get; init; } = null!;

        public string? Venue { get; init; }
        public string? Location { get; init; }
        
        public int? Attendance { get; init; }

        public IReadOnlyList<string> Broadcasts { get; init; } = [];
        public IReadOnlyList<FeaturedAthleteViewModel> FeaturedAthletes { get; init; } = [];
        
        public string? Headline { get; init; }
        public string? Recap { get; init; }
        
        public BaseballGameDetailsViewModel? Baseball { get; init; }
    }
}