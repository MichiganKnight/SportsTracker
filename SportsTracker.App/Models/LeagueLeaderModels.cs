namespace SportsTracker.App.Models
{
    public sealed class LeagueLeaders
    {
        public int Season { get; init; }
        
        public string SeasonName { get; init; } = string.Empty;
        
        public IReadOnlyList<LeaderCategory> Categories { get; init; } = [];
    }
    
    public sealed class LeaderCategory
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public IReadOnlyList<StatLeader> Leaders { get; init; } = [];
    }
    
    public sealed class StatLeader
    {
        public int Rank { get; init; }
        
        public string DisplayValue { get; init; } = string.Empty;
        public double Value { get; init; }
        
        public string AthleteId { get; init; } = string.Empty;
        public string AthleteName { get; init; } = string.Empty;
        
        public string? Headshot { get; init; }
        
        public string TeamId { get; init; } = string.Empty;
        public string TeamName { get; init; } = string.Empty;
        public string TeamAbbreviation { get; init; } = string.Empty;
        
        public string? TeamLogo { get; init; }
    }
}