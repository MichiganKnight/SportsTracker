using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models.Rankings;
using SportsTracker.App.ViewModels.Rankings;

namespace SportsTracker.App.Mapping
{
    public interface IRankingsViewModelMapper
    {
        RankingsViewModel Map(League league, LeagueRankings rankings);
    }

    public sealed class RankingsViewModelMapper : IRankingsViewModelMapper
    {
        public RankingsViewModel Map(League league, LeagueRankings rankings)
        {
            LeagueInfo leagueInfo = LeagueConfiguration.Get(league);

            List<RankingPollViewModel> polls = rankings.Polls.Select(MapPoll).ToList();

            return new RankingsViewModel
            {
                League = league,
                LeagueName = leagueInfo.DisplayName,
                Season = rankings.Season,

                LastUpdatedUtc = GetLastUpdated(rankings.Polls),

                Polls = polls
            };
        }

        private static RankingPollViewModel MapPoll(RankingPoll poll)
        {
            return new RankingPollViewModel
            {
                Id = poll.Id,

                Name = poll.Name,
                ShortName = poll.ShortName,
                Type = poll.Type,
                WeekDisplayName = poll.WeekDisplayName,

                Date = poll.Date,
                LastUpdatedUtc = poll.LastUpdatedUtc,

                Teams = poll.Teams.Select(MapTeam).ToList()
            };
        }

        private static RankedTeamViewModel MapTeam(RankedTeam team)
        {
            return new RankedTeamViewModel
            {
                Rank = team.Rank,
                PreviousRank = team.PreviousRank,
                Points = team.Points,
                FirstPlaceVotes = team.FirstPlaceVotes,
                Trend = team.Trend,

                TeamId = team.TeamId,

                TeamName = team.TeamName,
                TeamAbbreviation = team.TeamAbbreviation,
                TeamLogo = team.TeamLogo,

                Conference = team.Conference,
                Record = team.Record
            };
        }

        private static DateTime? GetLastUpdated(IEnumerable<RankingPoll> polls)
        {
            DateTime[] dates = polls
                .Where(p => p.LastUpdatedUtc.HasValue)
                .Select(p => p.LastUpdatedUtc!.Value)
                .ToArray();
            
            return dates.Length == 0 ? null : dates.Max();
        }
    }
}