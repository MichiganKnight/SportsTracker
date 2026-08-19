using SportsTracker.App.Integrations.ESPN.DTOs.Athlete;
using SportsTracker.App.Models.AthleteInfo;

namespace SportsTracker.App.Integrations.ESPN.Mappers
{
    public static class AthleteOverviewMapper
    {
        public static AthleteOverview? Map(AthleteOverviewResponseDto dto)
        {
            return new AthleteOverview
            {
                StatisticsTitle = dto.Statistics?.DisplayName ?? string.Empty,
                
                StatColumns = MapStatColumns(dto.Statistics),
                StatRows = MapStatRows(dto.Statistics),
                
                LatestNews = MapLatestNews(dto.News),
                Analysis = MapAnalysis(dto.Rotowire),
                Awards = MapAwards(dto.Awards),
                Fantasy = MapFantasy(dto.Fantasy),
                
                GolfSeasonRankingsTitle = dto.SeasonRankings?.DisplayName,
                GolfSeasonRankings = MapGolfSeasonRankings(dto.SeasonRankings),
                GolfRecentTournaments = MapGolfRecentTournaments(dto.RecentTournaments)
            };
        }

        private static IReadOnlyList<AthleteOverviewStatColumn> MapStatColumns(AthleteOverviewStatisticsDto? dto)
        {
            if (dto is null)
            {
                return [];
            }
            
            int count = Math.Max(dto.Labels.Count, Math.Max(dto.Names.Count, dto.DisplayNames.Count));
            
            List<AthleteOverviewStatColumn> columns = [];

            for (int i = 0; i < count; i++)
            {
                string name = i < dto.Names.Count ? dto.Names[i] : string.Empty;
                string label = i < dto.Labels.Count ? dto.Labels[i] : name;
                string displayName = i < dto.DisplayNames.Count ? dto.DisplayNames[i] : label;
                
                columns.Add(new AthleteOverviewStatColumn
                {
                    Name = name,
                    Label = label,
                    DisplayName = displayName
                });
            }
            
            return columns;
        }

        private static IReadOnlyList<AthleteOverviewStatRow> MapStatRows(AthleteOverviewStatisticsDto? dto)
        {
            if (dto?.Splits is null)
            {
                return [];
            }
            
            int columnCount = Math.Max(dto.Labels.Count, Math.Max(dto.Names.Count, dto.DisplayNames.Count));
            
            return dto.Splits
                .Select(split => new AthleteOverviewStatRow
                {
                    DisplayName = split.DisplayName ?? string.Empty,
                    
                    Stats = NormalizeStats(split.Stats, columnCount)
                })
                .ToList();
        }

        private static AthleteNews? MapLatestNews(IReadOnlyList<AthleteNewsDto>? news)
        {
            AthleteNewsDto? latest = news?.OrderByDescending(item => item.LastModified).FirstOrDefault();

            if (latest is null)
            {
                return null;
            }

            return new AthleteNews
            {
                Headline = latest.Headline ?? string.Empty,
                Description = latest.Description,
                LastModified = latest.LastModified,

                Image = latest.Images.FirstOrDefault(image => !string.IsNullOrWhiteSpace(image.Url))?.Url
            };
        }

        private static AthleteAnalysis? MapAnalysis(AthleteRotowireDto? dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new AthleteAnalysis
            {
                Headline = dto.Headline ?? string.Empty,
                Story = dto.Story,
                Description = dto.Description,

                Published = ParseDate(dto.Published)
            };
        }

        private static IReadOnlyList<AthleteAward> MapAwards(IReadOnlyList<AthleteAwardDto>? awards)
        {
            if (awards is null)
            {
                return [];
            }

            return awards
                .Where(award => !string.IsNullOrWhiteSpace(award.Name))
                .Select(award => new AthleteAward
                {
                    Name = award.Name ?? string.Empty,
                    DisplayCount = award.DisplayCount,
                    Seasons = award.Seasons
                })
                .ToList();
        }

        private static AthleteFantasy? MapFantasy(AthleteFantasyDto? dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new AthleteFantasy
            {
                DraftRank = dto.DraftRank,
                PositionRank = dto.PositionRank,
                PercentOwned = dto.PercentOwned,
                Projection = dto.Projection
            };
        }

        private static IReadOnlyList<GolfSeasonRanking> MapGolfSeasonRankings(GolfSeasonRankingsDto? dto)
        {
            if (dto?.Categories is null)
            {
                return [];
            }
            
            return dto.Categories
                .Where(category => !string.IsNullOrWhiteSpace(category.DisplayValue))
                .Select(category => new GolfSeasonRanking
                {
                    Name = category.Name ?? string.Empty,
                    DisplayName = category.ShortDisplayName ?? category.DisplayName ?? category.Name ?? string.Empty,
                    Abbreviation = category.Abbreviation ?? string.Empty,
                    DisplayValue = category.DisplayValue ?? string.Empty,
                    
                    Rank = category.Rank,
                    RankDisplayValue = category.RankDisplayValue
                })
                .ToList();
        }

        private static IReadOnlyList<GolfRecentTournament> MapGolfRecentTournaments(IReadOnlyList<GolfRecentTournamentGroupDto>? groups)
        {
            if (groups is null)
            {
                return [];
            }
            
            return groups
                .SelectMany(group => group.EventStats)
                .Select(MapGolfRecentTournament)
                .Where(tournament => tournament is not null)
                .Cast<GolfRecentTournament>()
                .OrderByDescending(tournament => tournament.StartDate)
                .ToList();
        }

        private static GolfRecentTournament? MapGolfRecentTournament(GolfRecentTournamentEventDto dto)
        {
            GolfRecentTournamentCompetitorDto? competitor = dto.Competitions.FirstOrDefault()?.Competitors.FirstOrDefault();

            if (competitor is null)
            {
                return null;
            }

            string? scoreToPar = competitor.Stats.FirstOrDefault(stat => string.Equals(stat.Name, "scoreToPar", StringComparison.OrdinalIgnoreCase))?.DisplayValue;

            scoreToPar ??= competitor.Score?.DisplayValue;

            return new GolfRecentTournament
            {
                Id = dto.Id ?? string.Empty,

                Name = dto.Name ?? string.Empty,

                StartDate = dto.Date,
                EndDate = dto.EndDate,

                Position = competitor.Status?.Position?.DisplayName,

                ScoreToPar = scoreToPar,

                RoundScores = competitor.LineScores?.Items.Select(score => score.Value.HasValue ? Convert.ToInt32(score.Value.Value) : (int?)null).ToList() ?? []
            };
        }

        private static IReadOnlyList<string> NormalizeStats(IReadOnlyList<string>? stats, int columnCount)
        {
            if (columnCount <= 0)
            {
                return [];
            }
            
            List<string> values = stats?.ToList() ?? [];

            while (values.Count < columnCount)
            {
                values.Add(string.Empty);
            }

            if (values.Count > columnCount)
            {
                values = values.Take(columnCount).ToList();
            }
            
            return values;
        }

        private static DateTime? ParseDate(string? value)
        {
            return DateTime.TryParse(value, out DateTime date) ? date : null;
        }
    }
}