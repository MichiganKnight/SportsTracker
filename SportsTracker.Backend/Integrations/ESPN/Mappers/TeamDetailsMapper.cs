using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class TeamDetailsMapper
    {
        public static TeamDetails? Map(TeamDetailsResponseDto response, League league)
        {
            TeamDetailsDto? team = response.Team;

            if (team is null || string.IsNullOrWhiteSpace(team.Id))
            {
                return null;
            }

            return new TeamDetails
            {
                Id = team.Id,
                League = league,

                Name = team.Name ?? string.Empty,
                DisplayName = team.DisplayName ?? team.Name ?? string.Empty,
                ShortDisplayName = team.ShortDisplayName ?? team.Name ?? string.Empty,
                Location = team.Location ?? string.Empty,
                Abbreviation = team.Abbreviation ?? string.Empty,

                Logo = GetLogo(team.Logos, "full", "default"),
                DarkLogo = GetLogo(team.Logos, "full", "dark"),

                Color = NormalizeColor(team.Color),
                AlternateColor = NormalizeColor(team.AlternateColor),

                IsActive = team.IsActive ?? true,

                GroupId = team.Groups?.Id,

                Records = MapRecords(team.Record),

                Venue = MapVenue(team.Franchise?.Venue)
            };
        }

        private static IReadOnlyList<TeamRecord> MapRecords(TeamRecordContainerDto? record)
        {
            if (record?.Items is null || record.Items.Count == 0)
            {
                return [];
            }
            
            return record.Items
                .Where(item => !string.IsNullOrWhiteSpace(item.Type) && !string.IsNullOrWhiteSpace(item.Summary))
                .Select(item => new TeamRecord
                {
                    Type = item.Type!,
                    Description = item.Description ?? item.Type!,
                    Summary = item.Summary!
                })
                .ToList();
        }

        private static TeamVenue? MapVenue(TeamVenueDto? venue)
        {
            if (venue is null)
            {
                return null;
            }

            return new TeamVenue
            {
                Id = venue.Id ?? string.Empty,

                Name = venue.FullName ?? venue.ShortName ?? string.Empty,

                City = venue.Address?.City,
                State = venue.Address?.State,
                ZipCode = venue.Address?.ZipCode,

                Grass = venue.Grass,
                Indoor = venue.Indoor,

                Image = GetVenueImage(venue.Images)
            };
        }

        private static string? GetLogo(IReadOnlyList<TeamLogoDto> logos, params string[] requiredRelations)
        {
            return logos.FirstOrDefault(logo => requiredRelations.All(required => logo.Rel.Any(rel => rel.Equals(required, StringComparison.OrdinalIgnoreCase))))?.Href;
        }

        private static string? GetVenueImage(IReadOnlyList<TeamVenueImageDto> images)
        {
            return images.FirstOrDefault(image => image.Rel.Any(rel => rel.Equals("day", StringComparison.OrdinalIgnoreCase)))?.Href ?? images.FirstOrDefault()?.Href;
        }
        
        private static string? NormalizeColor(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return null;
            }

            string normalized = color.Trim().TrimStart('#');

            return $"#{normalized}";
        }
    }
}