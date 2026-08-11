using SportsTracker.Backend.Integrations.ESPN.DTOs.Common;

namespace SportsTracker.Backend.Integrations.ESPN.DTOs.GameDetails
{
    public sealed class GameDetailsCompetitorDto
    {
        public string? Id { get; init; }
        
        public string? HomeAway { get; init; }
        
        public bool? Winner { get; init; }
        
        public GameDetailsTeamDto? Team { get; init; }
        
        public string? Score { get; init; }
        
        public List<LineScoreDto>? LineScores { get; init; } = [];
        public List<GameDetailsStatDto>? Statistics { get; init; } = [];
        public List<ProbablePitcherDto>? Probables { get; init; } = [];
        
        public int? Hits { get; init; }
        public int? Errors { get; init; }
        
        public List<RecordDto>? Records { get; init; } = [];
    }
}