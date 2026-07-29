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
            services.Configure<EspnOptions>(configuration.GetSection(EspnOptions.SectionName));
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));

            services.AddMemoryCache();
            
            services.AddHttpClient<IEspnApiClient, EspnApiClient>();

            services.AddScoped<IScoreboardService, ScoreboardService>();
            services.AddScoped<IScoreboardRefreshService, ScoreboardRefreshService>();
            
            services.AddSingleton<ICacheService, MemoryCacheService>();

            services.AddHostedService<ScoreboardWorker>();
            
            return services;
        }
    }
}