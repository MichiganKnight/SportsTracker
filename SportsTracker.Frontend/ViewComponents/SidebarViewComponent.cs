using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.ViewModels.NavigationInfo;

namespace SportsTracker.Frontend.ViewComponents
{
    public sealed class SidebarViewComponent(INavigationMapper mapper) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            IReadOnlyList<NavigationItemViewModel> model = mapper.Map();
            
            return View(model);
        }
    }
}