using Microsoft.AspNetCore.Mvc;
using SportsTracker.App.Enums;
using SportsTracker.App.Mapping;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.NavigationInfo;

namespace SportsTracker.App.ViewComponents
{
    public sealed class SidebarViewComponent(INavigationViewModelMapper viewModelMapper, IScoreboardService scoreboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            League[] leagues = LeagueConfiguration.All.ToArray();
            
            Task<CachedScoreboard?>[] tasks = leagues.Select(league => scoreboardService.GetScoreboardAsync(league)).ToArray();
            
            CachedScoreboard?[] responses = await Task.WhenAll(tasks);
            
            Dictionary<League, CachedScoreboard> scoreboards = new();

            for (int i = 0; i < leagues.Length; i++)
            {
                CachedScoreboard? scoreboard = responses[i];

                if (scoreboard is not null)
                {
                    scoreboards[leagues[i]] = scoreboard;
                }
            }
            
            IReadOnlyList<NavigationItemViewModel> model = viewModelMapper.Map(scoreboards);
            
            return View(model);
        }
    }
}