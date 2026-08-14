using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class TeamRosterMapper
    {
        public static TeamRoster Map(TeamRosterResponseDto dto, League league, string teamId)
        {
            return new TeamRoster
            {
                TeamId = teamId,
                League = league,

                Season = dto.Season?.Year,
                SeasonName = dto.Season?.Name,

                Groups = dto.Athletes.Where(group => group.Items.Count > 0).Select(MapGroup).ToList()
            };
        }

        private static RosterGroup MapGroup(TeamRosterGroupDto group)
        {
            return new RosterGroup
            {
                Name = group.Position ?? "Roster",

                Players = group.Items
                    .Select(MapPlayer)
                    .OrderBy(player => ParseJerseyNumber(player.Jersey))
                    .ThenBy(player => player.LastName)
                    .ToList()
            };
        }

        private static RosterPlayer MapPlayer(RosterAthleteDto athlete)
        {
            return new RosterPlayer
            {
                Id = athlete.Id ?? string.Empty,

                FirstName = athlete.FirstName ?? string.Empty,
                LastName = athlete.LastName ?? string.Empty,
                DisplayName = athlete.DisplayName ?? athlete.FullName ?? string.Empty,
                ShortName = athlete.ShortName ?? athlete.DisplayName ?? string.Empty,

                Jersey = athlete.Jersey,
                Headshot = athlete.Headshot?.Href,

                Position = athlete.Position?.DisplayName ?? athlete.Position?.Name,
                PositionAbbreviation = athlete.Position?.Abbreviation,

                Age = athlete.Age,
                DateOfBirth = athlete.BirthDate,

                Height = athlete.DisplayHeight,
                Weight = athlete.DisplayWeight,

                ExperienceYears = athlete.Experience?.Years,
                Status = athlete.Status?.Name,
                Bats = athlete.Bats?.Abbreviation,
                Throws = athlete.Throws?.Abbreviation,

                BirthPlace = athlete.BirthPlace?.DisplayText
            };
        }

        private static int ParseJerseyNumber(string? jersey)
        {
            return int.TryParse(jersey, out int number) ? number : int.MaxValue;
        }
    }
}