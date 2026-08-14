using SportsTracker.Frontend.ViewModels.TeamInfo;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class TeamRosterMapper : ITeamRosterMapper
    {
        public TeamRosterPageViewModel Map(TeamDetailsViewModel team, TeamRoster roster)
        {
            return new TeamRosterPageViewModel
            {
                Team = team,

                Season = roster.Season,
                SeasonName = roster.SeasonName,

                Groups = roster.Groups
                    .Select(group => new RosterGroupViewModel()
                    {
                        Name = group.Name,

                        Players = group.Players.Select(player => new RosterPlayerViewModel
                            {
                                Id = player.Id,
                                DisplayName = player.DisplayName,
                                Jersey = player.Jersey,
                                Headshot = player.Headshot,
                                Position = player.Position,
                                PositionAbbreviation = player.PositionAbbreviation,
                                Age = player.Age,
                                Height = player.Height,
                                Weight = player.Weight,
                                ExperienceYears = player.ExperienceYears,
                                Status = player.Status,
                                Bats = player.Bats,
                                Throws = player.Throws,
                                BirthPlace = player.BirthPlace
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}