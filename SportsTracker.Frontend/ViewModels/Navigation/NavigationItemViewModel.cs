using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.Navigation
{
    public sealed class NavigationItemViewModel
    {
        public League League { get; init; }
        
        public string Name { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
    }
}