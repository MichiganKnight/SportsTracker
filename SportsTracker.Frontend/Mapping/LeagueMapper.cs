using SportsTracker.Frontend.ViewModels.Dashboard;
using SportsTracker.Frontend.ViewModels.Pages;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class LeagueMapper : BaseMapper, ILeagueMapper
    {
        public LeaguePageViewModel Map(CachedScoreboard scoreboard)
        {
            League league = scoreboard.League;
            LeagueInfo info = LeagueConfiguration.Get(league);

            List<GameCardViewModel> games = scoreboard.Games.Select(MapGame).ToList();

            return new LeaguePageViewModel
            {
                League = league,

                LeagueName = info.DisplayName,
                Icon = info.Icon,

                LastUpdatedUtc = scoreboard.LastUpdatedUtc,
                
                Live = new GameSectionViewModel
                {
                    Games = games.Where(g => g.IsLive).ToList()
                },
                Upcoming = new GameSectionViewModel
                {
                    Games = games.Where(g => g.IsUpcoming).ToList()
                },
                Final = new GameSectionViewModel
                {
                    Games = games.Where(g => g.IsFinal).ToList()
                }
            };
        }
    }
}