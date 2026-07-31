namespace SportsTracker.Frontend.ViewModels.Navigation
{
    public sealed class NavigationItemViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        
        public bool Active { get; init; }
    }
}