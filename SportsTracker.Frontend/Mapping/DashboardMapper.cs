using SportsTracker.Frontend.ViewModels;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.Mapping
{
    public sealed class DashboardMapper : IDashboardMapper
    {
        public DashboardViewModel Map(IReadOnlyDictionary<League, IReadOnlyList<Game>> scoreboards)
        {
            List<LeagueSectionViewModel> sections = [];

            foreach (League league in LeagueConfiguration.All.OrderBy(l => LeagueConfiguration.Get(League.CFB).DisplayOrder))
            {
                LeagueInfo info = LeagueConfiguration.Get(league);

                scoreboards.TryGetValue(league, out IReadOnlyList<Game>? games);
                
                sections.Add(new LeagueSectionViewModel
                {
                    League = league,
                    LeagueName = info.DisplayName,
                    Icon = info.Icon,
                    Route = "/" + info.Route,
                    
                    Games = (games ?? []).Select(MapGame).ToList()
                });
            }

            return new DashboardViewModel
            {
                Leagues = sections
            };
        }

        private static GameCardViewModel MapGame(Game game)
        {
            return new GameCardViewModel
            {
                GameId = game.Id,

                AwayTeam = new TeamViewModel
                {
                    Id = game.AwayTeam.Id,
                    Name = game.AwayTeam.Abbreviation,
                    DisplayName = game.AwayTeam.DisplayName,
                    Abbreviation = game.AwayTeam.Abbreviation,
                    Logo = game.AwayTeam.Logo?.Href,
                    Record = game.AwayTeam.Record?.Summary,
                    Score = game.AwayScore,
                    PrimaryColor = game.AwayTeam.Color,
                    AlternateColor = game.AwayTeam.AlternateColor
                },

                HomeTeam = new TeamViewModel
                {
                    Id = game.HomeTeam.Id,
                    Name = game.HomeTeam.Abbreviation,
                    DisplayName = game.HomeTeam.DisplayName,
                    Abbreviation = game.HomeTeam.Abbreviation,
                    Logo = game.HomeTeam.Logo?.Href,
                    Record = game.HomeTeam.Record?.Summary,
                    Score = game.HomeScore,
                    PrimaryColor = game.HomeTeam.Color,
                    AlternateColor = game.HomeTeam.AlternateColor
                },

                Status = game.StatusText,

                IsLive = game.IsLive
            };
        }
    }
}