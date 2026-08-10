using System.Net;
using Microsoft.Extensions.Options;
using SportsTracker.Backend.Cache;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Services.Implementations;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Backend.Workers;

namespace SportsTracker.Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSportsTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EspnOptions>(configuration.GetSection(EspnOptions.SectionName));
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));

            services.AddMemoryCache();
            
            services.AddHttpClient<IEspnApiClient, EspnApiClient>((serviceProvider, client) =>
            {
                EspnOptions options = serviceProvider.GetRequiredService<IOptions<EspnOptions>>().Value;
                
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.53.0");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            });

            services.AddScoped<IScoreboardService, ScoreboardService>();
            services.AddScoped<IScoreboardRefreshService, ScoreboardRefreshService>();
            
            services.AddScoped<IStandingsService, StandingsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IStandingsGroupingService, StandingsGroupingService>();
            
            services.AddSingleton<ICacheService, MemoryCacheService>();

            services.AddHostedService<ScoreboardWorker>();
        }
    }
}