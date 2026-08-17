using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.Standings
{
    public sealed class LeagueStandings
    {
        public League League { get; init; }
        
        public int Season { get; init; }
        
        public IReadOnlyList<StandingsGroup> Groups { get; init; } = [];
    }
    
    public sealed class StandingsGroup
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public StandingsGroupType Type { get; init; }
        
        public IReadOnlyList<TeamStanding> Teams { get; init; } = [];
    }
    
    public sealed class TeamStanding
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
    }
}