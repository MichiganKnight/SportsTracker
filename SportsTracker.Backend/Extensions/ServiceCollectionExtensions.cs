using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;
using SportsTracker.Backend.Services.Implementations;
using SportsTracker.Backend.Services.Interfaces;

namespace SportsTracker.Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportsTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EspnOptions>(configuration.GetSection(EspnOptions.SectionName));
            
            services.AddHttpClient<IEspnApiClient, EspnApiClient>();

            services.AddScoped<IScoreboardService, ScoreboardService>();
            
            return services;
        }
    }
}