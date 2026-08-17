using SportsTracker.App.Enums;

namespace SportsTracker.App.Metadata
{
    public static class LeagueConfiguration
    {
        public static readonly IReadOnlyDictionary<League, LeagueInfo> Leagues = new Dictionary<League, LeagueInfo>
        {
            {
                League.NFL,
                new LeagueInfo
                {
                    League = League.NFL,
                    Sport = Sport.Football,
                    EspnSport = "football",
                    EspnLeague = "nfl",
                    DisplayName = "NFL",
                    Icon = "🏈"
                }
            },
            {
                League.CFB,
                new LeagueInfo
                {
                    League = League.CFB,
                    Sport = Sport.Football,
                    EspnSport = "football",
                    EspnLeague = "college-football",
                    DisplayName = "College Football",
                    Icon = "🎓"
                }
            },
            {
                League.NBA,
                new LeagueInfo
                {
                    League = League.NBA,
                    Sport = Sport.Basketball,
                    EspnSport = "basketball",
                    EspnLeague = "nba",
                    DisplayName = "NBA",
                    Icon = "🏀"
                }
            },
            {
                League.CBB,
                new LeagueInfo
                {
                    League = League.CBB,
                    Sport = Sport.Basketball,
                    EspnSport = "basketball",
                    EspnLeague = "mens-college-basketball",
                    DisplayName = "College Basketball",
                    Icon = "🎓"
                }
            },
            {
                League.MLB,
                new LeagueInfo
                {
                    League = League.MLB,
                    Sport = Sport.Baseball,
                    EspnSport = "baseball",
                    EspnLeague = "mlb",
                    DisplayName = "MLB",
                    Icon = "⚾"
                }
            },
            {
                League.NHL,
                new LeagueInfo
                {
                    League = League.NHL,
                    Sport = Sport.Hockey,
                    EspnSport = "hockey",
                    EspnLeague = "nhl",
                    DisplayName = "NHL",
                    Icon = "🏒"
                }
            },
            {
                League.PGA,
                new LeagueInfo
                {
                    League = League.PGA,
                    Sport = Sport.Golf,
                    EspnSport = "golf",
                    EspnLeague = "pga",
                    DisplayName = "PGA Tour",
                    Icon = "⛳"
                }
            }
        };
        
        public static IEnumerable<League> All => Leagues.Keys;
        
        public static LeagueInfo Get(League league) => Leagues[league];
    }
}