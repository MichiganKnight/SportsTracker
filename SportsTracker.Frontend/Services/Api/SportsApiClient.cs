using System.Net;
using Microsoft.Extensions.Options;
using SportsTracker.Frontend.Config;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Models;
using SportsTracker.Shared.Models.BoxScore;
using SportsTracker.Shared.Models.GameDetails;
using SportsTracker.Shared.Models.GameInfo;
using SportsTracker.Shared.Models.PlayByPlay;
using SportsTracker.Shared.Models.Standings;
using SportsTracker.Shared.Models.TeamInfo;

namespace SportsTracker.Frontend.Services.Api
{
    public class SportsApiClient : IScoreboardApiClient, IGameApiClient, IStandingsApiClient, ITeamApiClient
    {
        private readonly HttpClient _httpClient;

        public SportsApiClient(HttpClient httpClient, IOptions<SportsApiOptions> options)
        {
            _httpClient = httpClient;
            
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<ApiResponse<IReadOnlyList<Game>>?> GetScoreboardAsync(League league, CancellationToken cancellationToken = default)
        {
            ApiResponse<CachedScoreboard>? response = await GetLeagueAsync(league, cancellationToken);

            if (response?.Data is null)
            {
                return null;
            }

            return new ApiResponse<IReadOnlyList<Game>>
            {
                Data = response.Data.Games,
                TimestampUtc = response.TimestampUtc,
                Version = response.Version
            };
        }

        public Task<ApiResponse<CachedScoreboard>?> GetLeagueAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<CachedScoreboard>>((SportsApiEndpoints.League(league)), cancellationToken);
        }

        public Task<ApiResponse<LeagueStandings>?> GetStandingsAsync(League league, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<LeagueStandings>>((SportsApiEndpoints.Standings(league)), cancellationToken);
        }

        public Task<ApiResponse<GameDetails>?> GetGameDetailsAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<GameDetails>>((SportsApiEndpoints.Game(league, gameId)), cancellationToken);
        }

        public Task<ApiResponse<GameBoxScore>?> GetBoxScoreAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<GameBoxScore>>((SportsApiEndpoints.BoxScore(league, gameId)), cancellationToken);
        }

        public Task<ApiResponse<GamePlayByPlay>?> GetPlayByPlayAsync(League league, string gameId, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<GamePlayByPlay>>((SportsApiEndpoints.PlayByPlay(league, gameId)), cancellationToken);       
        }

        public Task<ApiResponse<TeamDetails>?> GetTeamDetailsAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<TeamDetails>>((SportsApiEndpoints.Team(league, teamId)), cancellationToken);      
        }
        
        public Task<ApiResponse<TeamSchedule>?> GetTeamScheduleAsync(League league, string teamId, CancellationToken cancellationToken = default)
        {
            return GetAsync<ApiResponse<TeamSchedule>>((SportsApiEndpoints.TeamSchedule(league, teamId)), cancellationToken);      
        }

        public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpClient.GetAsync(SportsApiEndpoints.Health, cancellationToken);
                
                Console.WriteLine($"Health Check: {_httpClient.BaseAddress}{SportsApiEndpoints.Health} -> {(int)response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Health Check Failed: {ex.Message}");
                
                return false;
            }
        }

        private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(relativeUrl, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }
    }
}