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
        
        private readonly Dictionary<League, DateTime> _nextRefreshUtc = [];

        public ScoreboardWorker(IServiceProvider serviceProvider, IOptions<CacheOptions> cacheOptions, ILogger<ScoreboardWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scoreboard Worker Started...");

            InitializeRefreshSchedule();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshDueLeagues(stoppingToken);

                    await Task.Delay(TimeSpan.FromSeconds(_cacheOptions.WorkerRefreshSeconds), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected Worker Error");
                }
            }
            
            _logger.LogInformation("Scoreboard Worker Stopped...");
        }

        private void InitializeRefreshSchedule()
        {
            DateTime now = DateTime.UtcNow;

            foreach (League league in LeagueConfiguration.All)
            {
                _nextRefreshUtc[league] = now;
            }
        }

        private async Task RefreshDueLeagues(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            
            IScoreboardRefreshService refreshService = scope.ServiceProvider.GetRequiredService<IScoreboardRefreshService>();
            
            DateTime now = DateTime.UtcNow;

            foreach (League league in LeagueConfiguration.All)
            {
                if (!_nextRefreshUtc.TryGetValue(league, out DateTime nextRefreshUtc))
                {
                    nextRefreshUtc = now;
                }
                
                if (now < nextRefreshUtc)
                {
                    continue;
                }

                try
                {
                    TimeSpan? refreshInterval = await refreshService.RefreshAsync(league, cancellationToken);

                    if (refreshInterval.HasValue)
                    {
                        _nextRefreshUtc[league] = DateTime.UtcNow + refreshInterval.Value;
                    }
                    else
                    {
                        ScheduleRetry(league);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed Refreshing {League}", league);
                    
                    ScheduleRetry(league);
                }
            }
        }
        
        private void ScheduleRetry(League league)
        {
            TimeSpan retryInterval = TimeSpan.FromMinutes(1);
            
            _nextRefreshUtc[league] = DateTime.UtcNow + retryInterval;
            
            _logger.LogWarning("{League} Refresh Failed | Retring in {Interval}", league, retryInterval);
        }
    }
}