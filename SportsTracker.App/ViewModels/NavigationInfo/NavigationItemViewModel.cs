using SportsTracker.App.Enums;

namespace SportsTracker.App.ViewModels.NavigationInfo
{
    public sealed class NavigationItemViewModel
    {
        public League League { get; init; }
        
        public string Name { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public string? Logo { get; init; }
        public string? DarkLogo { get; init; }
    }
}