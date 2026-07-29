using SportsTracker.Backend.Config;
using SportsTracker.Backend.Integrations.ESPN;

namespace SportsTracker.Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSportsTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EspnOptions>(configuration.GetSection(EspnOptions.SectionName));
            
            services.AddHttpClient<IEspnApiClient, EspnApiClient>();
            
            return services;
        }
    }
}