using SportsTracker.App.Models.TeamInfo;
using SportsTracker.App.ViewModels.TeamInfo;

namespace SportsTracker.App.Mapping
{
    public interface ITeamDetailsViewModelMapper
    {
        TeamDetailsViewModel Map(TeamDetails team);
    }
    
    public sealed class TeamDetailsViewModelMapper : ITeamDetailsViewModelMapper
    {
        public TeamDetailsViewModel Map(TeamDetails team)
        {
            return new TeamDetailsViewModel
            {
                League = team.League,

                Team = new TeamViewModel
                {
                    Id = team.Id,
                    Name = team.Name,
                    DisplayName = team.DisplayName,
                    ShortDisplayName = team.ShortDisplayName,
                    Abbreviation = team.Abbreviation,

                    Logo = team.Logo,
                    Record = GetRecord(team, "total"),

                    PrimaryColor = team.Color ?? string.Empty,
                    AlternateColor = team.AlternateColor
                },

                DarkLogo = team.DarkLogo,

                IsActive = team.IsActive,
                GroupId = team.GroupId,

                Records = team.Records
                    .Select(record => new TeamRecordViewModel
                    {
                        Type = record.Type,
                        Description = record.Description,
                        Summary = record.Summary
                    })
                    .ToList(),

                Venue = team.Venue is null
                    ? null
                    : new TeamVenueViewModel
                    {
                        Id = team.Venue.Id,
                        Name = team.Venue.Name,
                        City = team.Venue.City,
                        State = team.Venue.State,
                        ZipCode = team.Venue.ZipCode,
                        Grass = team.Venue.Grass,
                        Indoor = team.Venue.Indoor,
                        Image = team.Venue.Image
                    }
            };
        }

        private static string? GetRecord(TeamDetails team, string type)
        {
            return team.Records.FirstOrDefault(record => record.Type.Equals(type, StringComparison.OrdinalIgnoreCase))?.Summary;
        }
    }
}