using System.Net;
using Microsoft.Extensions.Options;
using SportsTracker.App.Cache;
using SportsTracker.App.Config;
using SportsTracker.App.Integrations.ESPN;
using SportsTracker.App.Mapping;
using SportsTracker.App.Services;
using SportsTracker.App.Workers;

namespace SportsTracker.App.Extensions
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
            services.Configure<SportsApiOptions>(configuration.GetSection(SportsApiOptions.SectionName));
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
                    SportsApiOptions options = serviceProvider.GetRequiredService<IOptions<SportsApiOptions>>().Value;
                
                    ConfigureEspnClient(client, options.BaseUrl);
                })
                .ConfigurePrimaryHttpMessageHandler(CreateEspnHandler);
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IGameCardViewModelMapper, GameCardViewModelMapper>();
            services.AddScoped<IDashboardViewModelMapper, DashboardViewModelMapper>();
            services.AddScoped<ILeagueViewModelMapper, LeagueViewModelMapper>();
            services.AddScoped<INavigationViewModelMapper, NavigationViewModelMapper>();
            services.AddScoped<IStandingsViewModelMapper, StandingsViewModelMapper>();
            services.AddScoped<IGameDetailsViewModelMapper, GameDetailsViewModelMapper>();
            services.AddScoped<IBoxScoreViewModelMapper, BoxScoreViewModelMapper>();
            services.AddScoped<IPlayByPlayViewModelMapper, PlayByPlayViewModelMapper>();
            services.AddScoped<ITeamDetailsViewModelMapper, TeamDetailsViewModelMapper>();
            services.AddScoped<ITeamRosterViewModelMapper, TeamRosterViewModelMapper>();
            services.AddScoped<IGolfEventCardViewModelMapper, GolfEventCardViewModelMapper>();
            services.AddScoped<IGolfTournamentViewModelMapper, GolfTournamentViewModelMapper>();
            services.AddScoped<IAthleteDetailsViewModelMapper, AthleteDetailsViewModelMapper>();
            
            services.AddScoped<IScoreboardService, ScoreboardService>();
            services.AddScoped<IScoreboardRefreshService, ScoreboardRefreshService>();
            
            services.AddScoped<IStandingsService, StandingsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            
            services.AddScoped<IGameDetailsService, GameDetailsService>();
            services.AddScoped<IGameSummaryService, GameSummaryService>();
            
            services.AddScoped<IGameContentService, GameContentService>();
            
            services.AddScoped<ITeamDetailsService, TeamDetailsService>();
            services.AddScoped<ITeamScheduleService, TeamScheduleService>();
            services.AddScoped<ITeamRosterService, TeamRosterService>();
            
            services.AddScoped<IAthleteDetailsService, AthleteDetailsService>();
            services.AddScoped<IAthleteOverviewService, AthleteOverviewService>();
        }

        private static void AddBackgroundServices(IServiceCollection services)
        {
            services.AddHostedService<ScoreboardWorker>();
        }

        private static void ConfigureEspnClient(HttpClient client, string baseUrl)
        {
            client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
            
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.53.0");
        }

        private static HttpClientHandler CreateEspnHandler()
        {
            return new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            };
        }
    }
}