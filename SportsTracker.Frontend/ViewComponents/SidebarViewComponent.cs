using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;
using SportsTracker.Frontend.ViewModels.NavigationInfo;
using SportsTracker.Shared.Common;
using SportsTracker.Shared.Enums;
using SportsTracker.Shared.Metadata;
using SportsTracker.Shared.Models;

namespace SportsTracker.Frontend.ViewComponents
{
    public sealed class SidebarViewComponent(INavigationMapper mapper, IScoreboardApiClient api) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            League[] leagues = LeagueConfiguration.All.ToArray();
            
            Task<ApiResponse<CachedScoreboard>?>[] tasks = leagues.Select(league => api.GetLeagueAsync(league)).ToArray();
            
            ApiResponse<CachedScoreboard>?[] responses = await Task.WhenAll(tasks);
            
            Dictionary<League, CachedScoreboard> scoreboards = new();

            for (int i = 0; i < leagues.Length; i++)
            {
                CachedScoreboard? scoreboard = responses[i]?.Data;

                if (scoreboard is not null)
                {
                    scoreboards[leagues[i]] = scoreboard;
                }
            }
            
            IReadOnlyList<NavigationItemViewModel> model = mapper.Map(scoreboards);
            
            return View(model);
        }
    }
}