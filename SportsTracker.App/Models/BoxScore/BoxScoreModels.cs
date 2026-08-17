using SportsTracker.App.Enums;

namespace SportsTracker.App.Models.BoxScore
{
    public sealed class GameBoxScore
    {
        public string GameId { get; init; } = string.Empty;
        
        public League League { get; init; }
        
        public IReadOnlyList<TeamBoxScore> Teams { get; init; } = [];
    }
    
    public sealed class TeamBoxScore
    {
        public string TeamId { get; init; } = string.Empty;
        public string TeamName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        
        public IReadOnlyList<PlayerStatTable> Tables { get; init; } = [];
    }
    
    public sealed class PlayerStatTable
    {
        public string Type { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<BoxScoreColumn> Columns { get; init; } = [];
        public IReadOnlyList<PlayerStatRow> Players { get; init; } = [];
        
        public IReadOnlyList<string> Totals { get; init; } = []; 
    }
    
    public sealed class PlayerStatRow
    {
        public string AthleteId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        public string? Position { get; init; }
        
        public bool Starter { get; init; }
        
        public int? BatOrder { get; init; }
        
        public string? Note { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }
    
    public sealed class BoxScoreColumn
    {
        public string Key { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        
        public string? Description { get; init; }
    }
}