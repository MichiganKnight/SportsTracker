using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Enums;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Integrations.ESPN.DTOs.News;
using SportsTracker.App.Integrations.ESPN.Mappers;
using SportsTracker.App.Models;
using SportsTracker.App.Models.LeagueNews;

namespace SportsTracker.App.Services
{
    public interface ILeagueNewsService
    {
        Task<LeagueNews?> GetNewsAsync(League league, CancellationToken cancellationToken = default);
        Task<LeagueNewsArticle?> GetArticleAsync(League league, string articleId, CancellationToken cancellationToken = default);
    }

    public sealed class LeagueNewsService(IEspnApiClient espnApiClient, ICacheService cache, IOptions<CacheOptions> cacheOptions, ILogger<TeamService> logger) : EspnCachedServiceBase(espnApiClient, cache), ILeagueNewsService
    {
        private readonly CacheOptions _cache = cacheOptions.Value;

        public Task<LeagueNews?> GetNewsAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<LeagueNewsResponseDto, LeagueNews>(league, $"News for {league}", CacheKeys.LeagueNews(league), EspnEndpoints.LeagueNews(league), dto => LeagueNewsMapper.Map(dto), _ => TimeSpan.FromMinutes(10), logger,
                cancellationToken);
        }

        public Task<LeagueNewsArticle?> GetArticleAsync(League league, string articleId, CancellationToken cancellationToken = default)
        {
            return GetOrFetchAsync<LeagueNewsArticleResponseDto, LeagueNewsArticle>(league, $"News Article {articleId}", CacheKeys.LeagueNewsArticle(articleId), EspnEndpoints.LeagueNewsArticle(articleId), LeagueNewsMapper.MapArticle,
                _ => TimeSpan.FromHours(6), logger, cancellationToken);
        }
    }
}