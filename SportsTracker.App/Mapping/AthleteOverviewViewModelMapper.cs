using SportsTracker.App.Models.AthleteInfo;
using SportsTracker.App.ViewModels.AthleteInfo;

namespace SportsTracker.App.Mapping
{
    public interface IAthleteOverviewViewModelMapper
    {
        AthleteOverviewViewModel Map(AthleteOverview overview);
    }
    
    public sealed class AthleteOverviewViewModelMapper : IAthleteOverviewViewModelMapper
    {
        public AthleteOverviewViewModel Map(AthleteOverview overview)
        {
            return new AthleteOverviewViewModel
            {
                StatisticsTitle = overview.StatisticsTitle,

                StatColumns = overview.StatColumns
                    .Select(column => new AthleteOverviewStatColumnViewModel
                    {
                        Label = column.Label,
                        DisplayName = column.DisplayName
                    })
                    .ToList(),

                StatRows = overview.StatRows
                    .Select(row => new AthleteOverviewStatRowViewModel
                    {
                        DisplayName = row.DisplayName,
                        Stats = row.Stats
                    })
                    .ToList(),

                LatestNews = overview.LatestNews is null
                    ? null
                    : new AthleteNewsViewModel
                    {
                        Headline = overview.LatestNews.Headline,
                        Description = overview.LatestNews.Description,
                        Image = overview.LatestNews.Image,
                        LastModified = overview.LatestNews.LastModified
                    },

                Analysis = overview.Analysis is null
                    ? null
                    : new AthleteAnalysisViewModel
                    {
                        Headline = overview.Analysis.Headline,
                        Story = overview.Analysis.Story,
                        Description = overview.Analysis.Description,
                        Published = overview.Analysis.Published
                    },

                Awards = overview.Awards
                    .Select(award => new AthleteAwardViewModel
                    {
                        Name = award.Name,
                        DisplayCount = award.DisplayCount,
                        Seasons = award.Seasons
                    })
                    .ToList(),

                Fantasy = overview.Fantasy is null
                    ? null
                    : new AthleteFantasyViewModel
                    {
                        DraftRank = overview.Fantasy.DraftRank,
                        PositionRank = overview.Fantasy.PositionRank,
                        PercentOwned = overview.Fantasy.PercentOwned,
                        Projection = overview.Fantasy.Projection
                    },

                GolfSeasonRankingsTitle = overview.GolfSeasonRankingsTitle,

                GolfSeasonRankings = overview.GolfSeasonRankings
                    .Select(ranking => new GolfSeasonRankingViewModel
                    {
                        DisplayName = ranking.DisplayName,
                        DisplayValue = ranking.DisplayValue,
                        Rank = ranking.RankDisplayValue
                    })
                    .ToList(),

                GolfRecentTournaments = overview.GolfRecentTournaments
                    .Select(tournament => new GolfRecentTournamentViewModel
                    {
                        Id = tournament.Id,
                        Name = tournament.Name,
                        StartDate = tournament.StartDate,
                        EndDate = tournament.EndDate,
                        Position = tournament.Position,
                        ScoreToPar = tournament.ScoreToPar,
                        RoundScores = tournament.RoundScores
                    })
                    .ToList()
            };
        }
    }
}