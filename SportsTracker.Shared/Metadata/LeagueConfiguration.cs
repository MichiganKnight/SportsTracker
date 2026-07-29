using SportsTracker.Shared.Enums;

namespace SportsTracker.Shared.Metadata
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
                    DisplayName = "NFL"
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
                    DisplayName = "College Football"
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
                    DisplayName = "NBA"
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
                    DisplayName = "College Basketball"
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
                    DisplayName = "MLB"
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
                    DisplayName = "NHL"
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
                    DisplayName = "PGA Tour"
                }
            }
        };
    }
}