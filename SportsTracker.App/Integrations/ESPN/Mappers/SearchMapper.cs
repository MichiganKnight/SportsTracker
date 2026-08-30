using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN.DTOs.Search;
using SportsTracker.App.Models;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static partial class SearchMapper
    {
        public static SearchResults Map(EspnSearchResponseDto dto, string query)
        {
            List<SearchResult> players = [];
            List<SearchResult> teams = [];
            List<SearchResult> games = [];

            foreach (EspnSearchGroupDto group in dto.Results)
            {
                switch (group.Type?.ToLowerInvariant())
                {
                    case "player":
                        players.AddRange(group.Contents.Select(MapPlayer).Where(result => result is not null).Cast<SearchResult>());
                        
                        break;
                    
                    case "team":
                        teams.AddRange(group.Contents.Select(MapTeam).Where(result => result is not null).Cast<SearchResult>());
                        
                        break;
                    
                    case "upcoming":
                        teams.AddRange(group.Contents.Select(MapGame).Where(result => result is not null).Cast<SearchResult>());
                        
                        break;
                }
            }
            
            return new SearchResults
            {
                Query = query,
                
                DidYouMean = dto.DidYouMean,
                
                Players = players,
                Teams = teams,
                Games = games
            };
        }

        private static SearchResult? MapPlayer(EspnSearchContentDto dto)
        {
            if (!TryMapLeague(dto.DefaultLeagueSlug, out League league))
            {
                return null;
            }

            string? athleteId = ExtractUidValue(dto.Uid, "a");

            if (string.IsNullOrWhiteSpace(athleteId))
            {
                return null;
            }

            return new SearchResult
            {
                Type = SearchResultType.Player,

                Id = athleteId,

                League = league,

                DisplayName = dto.DisplayName ?? string.Empty,
                Subtitle = dto.Subtitle,
                Description = dto.Description,

                Image = dto.Image?.Default,
                DarkImage = dto.Image?.DefaultDark
            };
        }

        private static SearchResult? MapTeam(EspnSearchContentDto dto)
        {
            if (!TryMapLeague(dto.DefaultLeagueSlug, out League league))
            {
                return null;
            }
            
            string? teamId = ExtractUidValue(dto.Uid, "t");
            
            if (string.IsNullOrWhiteSpace(teamId))
            {
                return null;
            }

            return new SearchResult
            {
                Type = SearchResultType.Team,

                Id = teamId,

                League = league,

                DisplayName = dto.DisplayName ?? string.Empty,
                Subtitle = dto.Subtitle,

                Image = dto.Image?.Default,
                DarkImage = dto.Image?.DefaultDark
            };
        }

        private static SearchResult? MapGame(EspnSearchContentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EventId))
            {
                return null;
            }
            
            League? league = ParseLeagueFromSubtitle(dto.Subtitle);

            if (!league.HasValue)
            {
                return null;
            }

            return new SearchResult
            {
                Type = SearchResultType.Game,

                Id = dto.EventId,

                League = league.Value,

                DisplayName = dto.DisplayName ?? string.Empty,

                Image = dto.Image?.Default,

                Date = dto.Date
            };
        }

        private static string? ExtractUidValue(string? uid, string key)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                return null;
            }

            string prefix = $"{key}:";

            string? part = uid
                .Split('~')
                .FirstOrDefault(value => value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            return part?[prefix.Length..];
        }

        private static bool TryMapLeague(string? slug, out League league)
        {
            league = default;

            if (string.IsNullOrWhiteSpace(slug))
            {
                return false;
            }

            return slug.ToLowerInvariant() switch
            {
                "nfl" => SetLeague(League.NFL, out league),
                "mlb" => SetLeague(League.MLB, out league),
                "college-football" => SetLeague(League.CFB, out league),
                "pga" => SetLeague(League.PGA, out league),

                _ => false
            };
        }

        private static League? ParseLeagueFromSubtitle(string? subtitle)
        {
            if (string.IsNullOrWhiteSpace(subtitle))
            {
                return null;
            }

            if (subtitle.Contains("NFL", StringComparison.OrdinalIgnoreCase))
            {
                return League.NFL;
            }

            if (subtitle.Contains("MLB", StringComparison.OrdinalIgnoreCase))
            {
                return League.MLB;
            }

            if (subtitle.Contains("College Football", StringComparison.OrdinalIgnoreCase) || subtitle.Contains("NCAAF", StringComparison.OrdinalIgnoreCase))
            {
                return League.CFB;
            }

            if (subtitle.Contains("PGA", StringComparison.OrdinalIgnoreCase))
            {
                return League.PGA;
            }
            
            return null;
        }

        private static bool SetLeague(League value, out League league)
        {
            league = value;
            
            return true;
        }
    }
}