namespace SportsTracker.App.Models.Rankings
{
    public sealed class LeagueRankings
    {
        public int Season { get; init; }
        
        public IReadOnlyList<RankingPoll> Polls { get; init; } = [];
    }

    public sealed class RankingPoll
    {
        public string Id { get; init; } = string.Empty;
        
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string WeekDisplayName { get; init; } = string.Empty;
        
        public DateTime? Date { get; init; }
        public DateTime? LastUpdatedUtc { get; init; }
        
        public IReadOnlyList<RankedTeam> Teams { get; init; } = [];
    }

    public sealed class RankedTeam
    {
        public int Rank { get; init; }
        public int PreviousRank { get; init; }
        public double Points { get; init; }
        public int FirstPlaceVotes { get; init; }
        
        public string Trend { get; init; } = string.Empty;
        
        public string TeamId { get; init; } = string.Empty;
        
        public string TeamName { get; init; } = string.Empty;
        public string TeamAbbreviation { get; init; } = string.Empty;
        
        public string? TeamLogo { get; init; }
        
        public string Conference { get; init; } = string.Empty;
        public string Record { get; init; } = string.Empty;
    }
}