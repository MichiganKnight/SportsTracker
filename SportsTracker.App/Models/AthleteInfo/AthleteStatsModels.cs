using System.Collections.ObjectModel;

namespace SportsTracker.App.Models.AthleteInfo
{
    public sealed class AthleteStats
    {
        public IReadOnlyList<AthleteStatsFilter> Filters { get; init; } = [];
        public IReadOnlyList<AthleteStatsCategory> Categories { get; init; } = [];
        
        public IReadOnlyDictionary<string, AthleteStatsTeam> Teams { get; init; } = new Dictionary<string, AthleteStatsTeam>();
        public IReadOnlyDictionary<string, string> Glossary { get; init; } = new Dictionary<string, string>();
    }

    public sealed class AthleteStatsFilter
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string SelectedValue { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteStatsFilterOption> Options { get; init; } = [];
    }

    public sealed class AthleteStatsFilterOption
    {
        public string Value { get; init; } = string.Empty;
        public string DisplayValue { get; init; } = string.Empty;
        public string ShortDisplayName { get; init; } = string.Empty;
    }

    public sealed class AthleteStatsCategory
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public IReadOnlyList<AthleteStatsColumn> Columns { get; init; } = [];
        public IReadOnlyList<AthleteStatsRow> Rows { get; init; } = [];
        
        public IReadOnlyList<string> Totals { get; init; } = [];
        public IReadOnlyList<string> Averages { get; init; } = [];
    }

    public sealed class AthleteStatsColumn
    {
        public string Name { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        
        public string? Description { get; init; }
    }

    public sealed class AthleteStatsRow
    {
        public string TeamId { get; init; } = string.Empty;
        
        public string TeamSlug { get; init; } = string.Empty;
        
        public int? SeasonYear { get; init; }
        public string Season { get; init; } = string.Empty;
        
        public string? Position { get; init; }
        
        public IReadOnlyList<string> Stats { get; init; } = [];
    }

    public sealed class AthleteStatsTeam
    {
        public string Id { get; init; } = string.Empty;
        
        public string DisplayName { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? DarkLogo { get; init; }
        
        public string? Color { get; init; }
        public string? AlternateColor { get; init; }
    }
}