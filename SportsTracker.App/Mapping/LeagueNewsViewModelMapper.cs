using SportsTracker.App.Enums;
using SportsTracker.App.Metadata;
using SportsTracker.App.Models;
using SportsTracker.App.Models.LeagueNews;
using SportsTracker.App.Services;
using SportsTracker.App.ViewModels.LeagueInfo;

namespace SportsTracker.App.Mapping
{
    public interface ILeagueNewsViewModelMapper
    {
        LeagueNewsViewModel Map(League league, LeagueNews news);
        LeagueNewsArticlePageViewModel MapArticle(League league, LeagueNewsArticle article);
    }
    
    public sealed class LeagueNewsViewModelMapper(ILeagueNewsArticleSanitizer leagueNewsArticleSanitizer) : ILeagueNewsViewModelMapper
    {
        public LeagueNewsViewModel Map(League league, LeagueNews news)
        {
            LeagueInfo leagueInfo = LeagueConfiguration.Get(league);

            return new LeagueNewsViewModel
            {
                League = league,
                LeagueName = leagueInfo.DisplayName,
                Header = news.Header,

                Articles = news.Articles.Select(MapArticle).ToList()
            };
        }

        public LeagueNewsArticlePageViewModel MapArticle(League league, LeagueNewsArticle article)
        {
            LeagueInfo leagueInfo = LeagueConfiguration.Get(league);
            
            LeagueNewsImage? image = article.Images.FirstOrDefault(image => string.Equals(image.Type, "header", StringComparison.OrdinalIgnoreCase)) ?? article.Images.FirstOrDefault();

            return new LeagueNewsArticlePageViewModel
            {
                League = league,
                LeagueName = leagueInfo.DisplayName,

                Id = article.Id,

                Type = article.Type,
                Headline = article.Headline,
                Description = article.Description,
                Story = leagueNewsArticleSanitizer.Sanitize(article.Story, league, article.References),
                Byline = article.Byline,

                Published = article.Published,
                LastModified = article.LastModified,

                Premium = article.Premium,

                ArticleUrl = article.ArticleUrl,

                ImageUrl = image?.Url ?? string.Empty,
                ImageAlt = image?.Alt ?? article.Headline,
                ImageCaption = image?.Caption ?? string.Empty,
                ImageCredit = image?.Credit ?? string.Empty
            };
        }

        private static LeagueNewsArticleViewModel MapArticle(LeagueNewsArticle article)
        {
            LeagueNewsImage? image = article.Images.FirstOrDefault(image => string.Equals(image.Type, "header", StringComparison.OrdinalIgnoreCase)) ?? article.Images.FirstOrDefault();

            return new LeagueNewsArticleViewModel
            {
                Id = article.Id,

                Type = article.Type,
                Headline = article.Headline,
                Description = article.Description,
                Byline = article.Byline,
                Published = article.Published,
                Premium = article.Premium,
                ArticleUrl = article.ArticleUrl,

                ImageUrl = image?.Url ?? string.Empty,
                ImageAlt = image?.Alt ?? article.Headline,
                ImageCredit = image?.Credit ?? string.Empty
            };
        }
    }
}