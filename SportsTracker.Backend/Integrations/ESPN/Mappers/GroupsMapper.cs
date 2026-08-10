using SportsTracker.Backend.Integrations.ESPN.DTOs.Groups;
using SportsTracker.Shared.Models.Groups;

namespace SportsTracker.Backend.Integrations.ESPN.Mappers
{
    public static class GroupsMapper
    {
        public static IReadOnlyList<SportsGroup> Map(GroupsResponseDto response)
        {
            return response.Groups?.Select(MapGroup).ToList() ?? [];
        }

        private static SportsGroup MapGroup(GroupDto dto)
        {
            return new SportsGroup
            {
                Name = dto.Name ?? string.Empty,
                Abbreviation = dto.Abbreviation ?? string.Empty,

                TeamIds = dto.Teams?.Where(team => !string.IsNullOrWhiteSpace(team.Id)).Select(team => team.Id!).ToList() ?? [],
                Children = dto.Children?.Select(MapGroup).ToList() ?? []
            };
        }
    }
}