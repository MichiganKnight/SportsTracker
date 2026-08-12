using SportsTracker.Frontend.ViewModels.BoxScore;
using SportsTracker.Shared.Models.BoxScore;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class BoxScoreMapper : IBoxScoreMapper
    {
        public BoxScoreViewModel Map(GameBoxScore boxScore)
        {
            return new BoxScoreViewModel
            {
                GameId = boxScore.GameId,
                League = boxScore.League,

                Teams = boxScore.Teams.Select(MapTeam).ToList()
            };
        }

        private static TeamBoxScoreViewModel MapTeam(TeamBoxScore team)
        {
            return new TeamBoxScoreViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Abbreviation = team.Abbreviation,
                Logo = team.Logo,
                
                Tables = team.Tables.Select(MapTable).ToList()
            };
        }

        private static PlayerStatTableViewModel MapTable(PlayerStatTable table)
        {
            return new PlayerStatTableViewModel
            {
                Type = table.Type,
                DisplayName = table.DisplayName,
                
                Columns = table.Columns
                    .Select(column => new BoxScoreColumnViewModel
                    {
                        Key = column.Key,
                        Label = column.Label,
                        Description = column.Description
                    }).ToList(),
                
                Players = table.Players
                    .Select(player => new PlayerStatRowViewModel
                    {
                        AthleteId = player.AthleteId,
                        Name = player.Name,
                        ShortName = player.ShortName,
                        Headshot = player.Headshot,
                        Position = player.Position,
                        Starter = player.Starter,
                        BatOrder = player.BatOrder,
                        Stats = player.Stats
                    }).ToList(),
                
                Totals = table.Totals
            };
        }
    }
}