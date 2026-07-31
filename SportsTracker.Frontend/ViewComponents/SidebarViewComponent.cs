using Microsoft.AspNetCore.Mvc;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.ViewModels.Navigation;

namespace SportsTracker.Frontend.ViewComponents
{
    public sealed class SidebarViewComponent : ViewComponent
    {
        private readonly INavigationMapper _mapper;
        
        public SidebarViewComponent(INavigationMapper mapper)
        {
            _mapper = mapper;
        }
        
        public IViewComponentResult Invoke()
        {
            IReadOnlyList<NavigationItemViewModel> model = _mapper.Map();
            
            return View(model);
        }
    }
}