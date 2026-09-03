using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.Rankings
{
    public sealed class RankingsViewModel
    {
        public League League { get; init; }

        public string LeagueName { get; init; } = string.Empty;

        public int Season { get; init; }

        public DateTime? LastUpdatedUtc { get; init; }

        public IReadOnlyList<RankingPollViewModel> Polls { get; init; } = [];
    }

    public sealed class RankingPollViewModel
    {
        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string WeekDisplayName { get; init; } = string.Empty;

        public DateTime? Date { get; init; }
        public DateTime? LastUpdatedUtc { get; init; }

        public IReadOnlyList<RankedTeamViewModel> Teams { get; init; } = [];
    }

    public sealed class RankedTeamViewModel
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