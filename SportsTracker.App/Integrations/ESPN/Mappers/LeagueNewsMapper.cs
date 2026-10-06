using SportsTracker.App.Integrations.ESPN.DTOs.News;
using SportsTracker.App.Models.LeagueNews;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public sealed class LeagueNewsMapper
    {
        public static LeagueNews Map(LeagueNewsResponseDto response)
        {
            return new LeagueNews
            {
                Header = response.Header ?? string.Empty,

                Articles = response.Articles.Where(article => !string.IsNullOrWhiteSpace(article.Headline)).Select(MapArticle).OrderByDescending(article => article.Published).ToList()
            };
        }

        public static LeagueNewsArticle? MapArticle(LeagueNewsArticleResponseDto response)
        {
            LeagueNewsArticleDto? article = response.Headlines.FirstOrDefault();
            
            return article is null ? null : MapArticle(article);
        }

        private static LeagueNewsArticle MapArticle(LeagueNewsArticleDto article)
        {
            return new LeagueNewsArticle
            {
                Id = article.Id?.ToString() ?? string.Empty,

                Type = article.Type ?? string.Empty,
                Headline = article.Headline ?? string.Empty,
                Description = article.Description ?? string.Empty,
                Story = article.Story ?? string.Empty,
                Byline = article.Byline ?? string.Empty,

                Published = article.Published,
                LastModified = article.LastModified,

                Premium = article.Premium ?? false,

                ArticleUrl = article.Links?.Web?.Href ?? string.Empty,

                Images = article.Images.Where(image => !string.IsNullOrWhiteSpace(image.Url)).Select(MapImage).ToList(),
                References = article.Categories.Select(MapReference).Where(reference => reference is not null).Cast<LeagueNewsReference>().ToList()
            };
        }

        private static LeagueNewsImage MapImage(LeagueNewsImageDto image)
        {
            return new LeagueNewsImage
            {
                Url = image.Url ?? string.Empty,
                Type = image.Type ?? string.Empty,
                Alt = image.Alt ?? string.Empty,
                Caption = image.Caption ?? string.Empty,
                Credit = image.Credit ?? string.Empty,

                Width = image.Width,
                Height = image.Height
            };
        }

        private static LeagueNewsReference? MapReference(LeagueNewsCategoryDto category)
        {
            string type = category.Type?.ToLowerInvariant() ?? string.Empty;

            return type switch
            {
                "athlete" when category.AthleteId.HasValue => new LeagueNewsReference
                {
                    Type = "athlete",
                    Id = category.AthleteId.Value.ToString(),
                    Description = category.Athlete?.Description ?? category.Description ?? string.Empty
                },
                
                "team" when category.TeamId.HasValue => new LeagueNewsReference
                {
                    Type = "team",
                    Id = category.TeamId.Value.ToString(),
                    Description = category.Team?.Description ?? category.Description ?? string.Empty,
                    Abbreviation = category.Team?.Abbreviation ?? string.Empty
                },
                
                "event" when category.EventId.HasValue => new LeagueNewsReference
                {
                    Type = "event",
                    Id = category.EventId.Value.ToString(),
                    Description = category.Event?.Description ?? category.Description ?? string.Empty
                },
                
                _ => null
            };
        }
    }
}