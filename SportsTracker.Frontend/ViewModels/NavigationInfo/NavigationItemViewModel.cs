using SportsTracker.Shared.Enums;

namespace SportsTracker.Frontend.ViewModels.NavigationInfo
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