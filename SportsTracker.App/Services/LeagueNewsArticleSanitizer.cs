using HtmlAgilityPack;
using SportsTracker.App.Enums;
using SportsTracker.App.Models.LeagueNews;

namespace SportsTracker.App.Services
{
    public interface ILeagueNewsArticleSanitizer
    {
        string Sanitize(string html, League league, IReadOnlyList<LeagueNewsReference> references);
    }

    public sealed class LeagueNewsArticleSanitizer : ILeagueNewsArticleSanitizer
    {
        private static readonly HashSet<string> AllowedTags =
        [
            "p",
            "a",
            "strong",
            "b",
            "em",
            "i",
            "ul",
            "ol",
            "li",
            "blockquote",
            "br",
            "h2",
            "h3",
            "h4"
        ];

        private static readonly HashSet<string> RemoveEntirely =
        [
            "script",
            "style",
            "iframe",
            "object",
            "embed",
            "alsosee"
        ];
        
        public string Sanitize(string html, League league, IReadOnlyList<LeagueNewsReference> references)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            HtmlDocument document = new();
            
            document.LoadHtml(html);
            
            SanitizeNodes(document);
            SanitizeAttributes(document);
            RewriteLinks(document, league, references);
            
            return document.DocumentNode.InnerHtml;
        }

        private static void SanitizeNodes(HtmlDocument document)
        {
            List<HtmlNode> nodes = document.DocumentNode.Descendants().Where(node => node.NodeType == HtmlNodeType.Element).Reverse().ToList();
            
            foreach (HtmlNode node in nodes)
            {
                string name = node.Name.ToLowerInvariant();

                if (AllowedTags.Contains(name))
                {
                    continue;
                }

                if (RemoveEntirely.Contains(name))
                {
                    node.Remove();
                    
                    continue;
                }
                
                UnwrapNode(node);
            }
        }

        private static void UnwrapNode(HtmlNode node)
        {
            HtmlNode? parent = node.ParentNode;

            if (parent is null)
            {
                return;
            }

            foreach (HtmlNode child in node.ChildNodes.ToList())
            {
                parent.InsertBefore(child, node);
            }
            
            parent.RemoveChild(node);
        }

        private static void SanitizeAttributes(HtmlDocument document)
        {
            foreach (HtmlNode node in document.DocumentNode.Descendants().Where(node => node.NodeType == HtmlNodeType.Element))
            {
                if (!node.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
                {
                    node.Attributes.RemoveAll();
                    
                    continue;
                }
                
                string? href = node.GetAttributeValue("href", string.Empty);
                
                node.Attributes.RemoveAll();

                if (!string.IsNullOrWhiteSpace(href))
                {
                    node.SetAttributeValue("href", href);
                }
            }
        }

        private static void RewriteLinks(HtmlDocument document, League league, IReadOnlyList<LeagueNewsReference> references)
        {
            List<HtmlNode> links = document.DocumentNode.Descendants("a").ToList();

            foreach (HtmlNode link in links)
            {
                string href = link.GetAttributeValue("href", string.Empty);

                if (string.IsNullOrWhiteSpace(href))
                {
                    continue;
                }
                
                string? internalUrl = TryCreateInternalUrl(href, league, references);

                if (!string.IsNullOrWhiteSpace(internalUrl))
                {
                    link.SetAttributeValue("href", internalUrl);
                    
                    continue;
                }
                
                SanitizeExternalLink(link, href);
            }
        }

        private static string? TryCreateInternalUrl(string href, League league, IReadOnlyList<LeagueNewsReference> references)
        {
            if (!Uri.TryCreate(href, UriKind.Absolute, out Uri? uri))
            {
                return null;
            }

            if (!IsEspnHost(uri.Host))
            {
                return null;
            }

            string[] segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            string? athleteUrl = TryCreateAthleteUrl(segments, league);
            if (athleteUrl is not null)
            {
                return athleteUrl;
            }
            
            string? teamUrl = TryCreateTeamUrl(segments, league, references);
            if (teamUrl is not null)
            {
                return teamUrl;
            }
            
            return TryCreateGameUrl(segments, league);
        }

        private static string? TryCreateAthleteUrl(string[] segments, League league)
        {
            int playerIndex = FindSegment(segments, "player");
            if (playerIndex < 0)
            {
                return null;
            }
            
            string? athleteId = GetValueAfterMarker(segments, playerIndex, "id");

            if (!IsNumericId(athleteId))
            {
                return null;
            }

            return $"/athlete/{league}/{athleteId}";
        }

        private static string? TryCreateTeamUrl(string[] segments, League league, IReadOnlyList<LeagueNewsReference> references)
        {
            int teamIndex = FindSegment(segments, "team");
            if (teamIndex < 0)
            {
                return null;
            }
            
            string? abbreviation = GetValueAfterMarker(segments, teamIndex, "name");

            if (string.IsNullOrWhiteSpace(abbreviation))
            {
                return null;
            }
            
            LeagueNewsReference? team = references.FirstOrDefault(reference => reference.Type.Equals("team", StringComparison.OrdinalIgnoreCase) && reference.Abbreviation.Equals(abbreviation, StringComparison.OrdinalIgnoreCase));

            if (team is null || !IsNumericId(team.Id))
            {
                return null;
            }
            
            return $"/team/{league}/{team.Id}";
        }

        private static string? TryCreateGameUrl(string[] segments, League league)
        {
            int gameIndex = FindSegment(segments, "game");
            if (gameIndex < 0)
            {
                return null;
            }
            
            string? gameId = GetValueAfterMarker(segments, gameIndex, "gameId");
            
            if (!IsNumericId(gameId))
            {
                return null;
            }
            
            return $"/game/{league}/{gameId}";
        }

        private static int FindSegment(string[] segments, string value)
        {
            return Array.FindIndex(segments, segment => segment.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        private static string? GetValueAfterMarker(string[] segments, int startIndex, string marker)
        {
            for (int index = startIndex; index < segments.Length - 1; index++)
            {
                if (!segments[index].Equals(marker, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                
                return segments[index + 1];
            }
            
            return null;
        }

        private static bool IsNumericId(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.All(char.IsDigit);
        }

        private static bool IsEspnHost(string host)
        {
            return host.Equals("espn.com", StringComparison.OrdinalIgnoreCase) || host.EndsWith(".espn.com", StringComparison.OrdinalIgnoreCase);
        }

        private static void SanitizeExternalLink(HtmlNode link, string href)
        {
            if (!Uri.TryCreate(href, UriKind.Absolute, out Uri? uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                link.Attributes.Remove("href");
                
                return;
            }
            
            link.SetAttributeValue("target", "_blank");
            link.SetAttributeValue("rel", "noopener noreferrer");
        }
    }
}