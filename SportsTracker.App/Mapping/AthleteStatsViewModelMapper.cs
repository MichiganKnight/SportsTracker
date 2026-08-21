using Microsoft.AspNetCore.Routing.Template;
using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Mapping
{
    public interface IAthleteStatsViewModelMapper
    {
        AthleteStatsViewModel Map(AthleteStats stats);
    }
    
    public class AthleteStatsViewModelMapper : IAthleteStatsViewModelMapper
    {
        public AthleteStatsViewModel Map(AthleteStats stats)
        {
            return new AthleteStatsViewModel
            {
                Categories = stats.Categories
                    .Select(category => MapCategory(category, stats.Teams))
                    .ToList()
            };
        }

        private static AthleteStatsCategoryViewModel MapCategory(AthleteStatsCategory category, IReadOnlyDictionary<string, AthleteStatsTeam> teams)
        {
            return new AthleteStatsCategoryViewModel
            {
                Name = category.Name,
                
                DisplayName = category.DisplayName,
                
                Columns = category.Columns
                    .Select(column => new AthleteStatsColumnViewModel
                    {
                        Label = column.Label,
                        DisplayName = column.DisplayName,
                        Description = column.Description
                    })
                    .ToList(),
                
                Rows = category.Rows
                    .OrderByDescending(row => row.SeasonYear)
                    .Select(row => MapRow(row, teams))
                    .ToList(),
                
                Totals = category.Totals
            };
        }

        private static AthleteStatsRowViewModel MapRow(AthleteStatsRow row, IReadOnlyDictionary<string, AthleteStatsTeam> teams)
        {
            teams.TryGetValue(row.TeamId, out AthleteStatsTeam? team);

            return new AthleteStatsRowViewModel
            {
                Season = row.Season,

                TeamId = row.TeamId,

                TeamName = team?.DisplayName ?? row.TeamSlug,
                TeamAbbreviation = team?.Abbreviation ?? string.Empty,

                TeamLogo = team?.Logo,
                TeamDarkLogo = team?.DarkLogo,

                Position = row.Position,

                Stats = row.Stats
            };
        } 
    }
}