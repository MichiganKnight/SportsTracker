using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.Season;
using SportsTracker.App.ViewModels.Season;
using SportsTracker.App.ViewModels.Standings;

namespace SportsTracker.App.Mapping
{
    public interface IPostseasonViewModelMapper
    {
        PostseasonViewModel Map(CachedScoreboard scoreboard, SeasonPeriod postseason, int selectedRound, SeasonSelectorViewModel seasonSelector);
    }
    
    public sealed class PostseasonViewModelMapper(IGameCardViewModelMapper gameCardViewModelMapper) : IPostseasonViewModelMapper
    {
        public PostseasonViewModel Map(CachedScoreboard scoreboard, SeasonPeriod postseason, int selectedRound, SeasonSelectorViewModel seasonSelector)
        {
            return new PostseasonViewModel
            {
                League = scoreboard.League,

                LeagueName = LeagueConfiguration.Get(scoreboard.League).DisplayName,
                
                SeasonSelector = seasonSelector,

                SelectedRound = selectedRound,

                Rounds = postseason.Weeks.Select(week => new PostseasonRoundViewModel
                    {
                        Number = week.Number,
                        Name = week.Name,
                        Detail = week.Detail,
                        StartDate = week.StartDate,
                        EndDate = week.EndDate
                    })
                    .ToList(),

                Games = scoreboard.Games.OrderBy(game => game.StartTime)
                    .Select(gameCardViewModelMapper.Map)
                    .ToList()
            };
        }
    }
}