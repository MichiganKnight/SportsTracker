using Microsoft.Extensions.Options;
using SportsTracker.Backend.Config;
using SportsTracker.Backend.Services.Interfaces;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;

namespace SportsTracker.Backend.Workers
{
    public sealed class ScoreboardWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CacheOptions _cacheOptions;
        private readonly ILogger<ScoreboardWorker> _logger;

        public ScoreboardWorker(IServiceProvider serviceProvider, IOptions<CacheOptions> cacheOptions, ILogger<ScoreboardWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scoreboard Worker Started...");
            
            await RefreshAllLeagues(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_cacheOptions.WorkerRefreshSeconds), stoppingToken);

                    await RefreshAllLeagues(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected Worker Error");
                }
                
                _logger.LogInformation("Scoreboard Worker Stopped...");
            }
        }

        private async Task RefreshAllLeagues(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            
            IScoreboardRefreshService refreshService = scope.ServiceProvider.GetRequiredService<IScoreboardRefreshService>();

            foreach (League league in LeagueConfiguration.All)
            {
                try
                {
                    await refreshService.RefreshAsync(league, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed Refreshing {League}", league);
                }
            }
        }
    }
}