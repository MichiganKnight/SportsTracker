using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs.Team;
using SportsTracker.App.Models.TeamInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class TeamsMapper
    {
        public static IReadOnlyList<TeamDetails> Map(TeamsResponseDto response, League league)
        {
            return response.Sports.SelectMany(sport => sport.Leagues)
                .SelectMany(leagueDto => leagueDto.Teams)
                .Select(entry => TeamDetailsMapper.Map(entry.Team, league))
                .Where(team => team is not null)
                .Cast<TeamDetails>()
                .ToList();
        }
    }
}