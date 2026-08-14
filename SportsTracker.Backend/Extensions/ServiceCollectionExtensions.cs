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
        public static IServiceCollection AddSportsTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddOptions(services, configuration);
            AddCaching(services);
            AddEspnIntegration(services);
            AddApplicationServices(services);
            AddBackgroundServices(services);
            
            return services;
        }

        private static void AddOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EspnOptions>(configuration.GetSection(EspnOptions.SectionName));
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));
        }

        private static void AddCaching(IServiceCollection services)
        {
            services.AddMemoryCache();
            
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
        
        private static void AddEspnIntegration(IServiceCollection services)
        {
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
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IScoreboardService, ScoreboardService>();
            services.AddScoped<IScoreboardRefreshService, ScoreboardRefreshService>();
            
            services.AddScoped<IStandingsService, StandingsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            
            services.AddScoped<IGameDetailsService, GameDetailsService>();
            services.AddScoped<IGameSummaryService, GameSummaryService>();
            
            services.AddScoped<IGameContentService, GameContentService>();
            
            services.AddScoped<ITeamDetailsService, TeamDetailsService>();
            services.AddScoped<ITeamScheduleService, TeamScheduleService>();
        }

        private static void AddBackgroundServices(IServiceCollection services)
        {
            services.AddHostedService<ScoreboardWorker>();
        }
    }
}