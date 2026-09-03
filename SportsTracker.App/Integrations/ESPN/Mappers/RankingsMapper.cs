using SportsTracker.App.Integrations.ESPN.DTOs;
using SportsTracker.App.Models.Rankings;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class RankingsMapper
    {
        private static readonly HashSet<string> SupportedPollTypes =
        [
            "ap",
            "usa",
            "cfp"
        ];

        public static LeagueRankings Map(RankingsResponseDto response, int season)
        {
            List<RankingPoll> polls = response.Rankings?.Where(IsSupportedPoll).Select(MapPoll).ToList() ?? [];

            return new LeagueRankings
            {
                Season = season,
                Polls = polls
            };
        }

        private static bool IsSupportedPoll(RankingPollDto poll)
        {
            if (string.IsNullOrWhiteSpace(poll.Type))
            {
                return false;
            }
            
            return SupportedPollTypes.Contains(poll.Type);
        }

        private static RankingPoll MapPoll(RankingPollDto poll)
        {
            List<RankedTeam> teams = poll.Ranks?
                .Select(MapTeam)
                .Where(team => team is not null)
                .Select(team => team!)
                .OrderBy(team => team.Rank)
                .ToList() ?? [];

            return new RankingPoll
            {
                Id = poll.Id ?? string.Empty,

                Name = poll.Name ?? string.Empty,
                ShortName = poll.ShortName ?? poll.Name ?? string.Empty,

                Type = poll.Type ?? string.Empty,

                WeekDisplayName = poll.Occurrence?.DisplayValue ?? string.Empty,

                Date = poll.Date,
                LastUpdatedUtc = poll.LastUpdated,

                Teams = teams
            };
        }

        private static RankedTeam? MapTeam(RankingEntryDto entry)
        {
            RankingTeamDto? team = entry.Team;

            return new RankedTeam
            {
                Rank = entry.Current ?? 0,
                PreviousRank = entry.Previous ?? 0,
                Points = entry.Points ?? 0,
                FirstPlaceVotes = entry.FirstPlaceVotes ?? 0,
                Trend = entry.Trend ?? string.Empty,

                TeamId = team.Id ?? string.Empty,

                TeamName = team.Location ?? team.Name ?? team.Nickname ?? string.Empty,
                TeamAbbreviation = team.Abbreviation ?? string.Empty,

                TeamLogo = team.Logo,

                Conference = team.Groups?.ShortName ?? string.Empty,

                Record = entry.RecordSummary ?? string.Empty,
            };
        }
    }
}