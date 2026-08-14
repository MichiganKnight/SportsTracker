using SportsTracker.Backend.Integrations.ESPN.DTOs.Team;

public sealed class TeamScheduleResponseDto
{
    public List<TeamScheduleEventDto> Events { get; init; } = [];
}

public sealed class TeamScheduleEventDto
{
    public string Id { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public List<TeamScheduleCompetitionDto> Competitions { get; init; } = [];
}

public sealed class TeamScheduleCompetitionDto
{
    public bool NeutralSite { get; init; }

    public TeamScheduleVenueDto? Venue { get; init; }

    public List<TeamScheduleCompetitorDto> Competitors { get; init; } = [];

    public TeamScheduleStatusDto? Status { get; init; }
}

public sealed class TeamScheduleCompetitorDto
{
    public string Id { get; init; } = string.Empty;

    public string HomeAway { get; init; } = string.Empty;

    public bool? Winner { get; init; }

    public TeamScheduleTeamDto Team { get; init; } = new();

    public TeamScheduleScoreDto? Score { get; init; }
    
    public List<TeamScheduleRecordDto> Record { get; init; } = [];
}

public sealed class TeamScheduleTeamDto
{
    public string Id { get; init; } = string.Empty;

    public string? Location { get; init; }
    public string? Abbreviation { get; init; }
    public string? DisplayName { get; init; }
    public string? ShortDisplayName { get; init; }

    public List<TeamLogoDto> Logos { get; init; } = [];
}

public sealed class TeamScheduleScoreDto
{
    public double? Value { get; init; }
    public string? DisplayValue { get; init; }
}

public sealed class TeamScheduleRecordDto
{
    public string? Description { get; init; }
    public string? Type { get; init; }
    public string? DisplayValue { get; init; }
}

public sealed class TeamScheduleVenueDto
{
    public string? FullName { get; init; }
}

public sealed class TeamScheduleStatusDto
{
    public TeamScheduleStatusTypeDto Type { get; init; } = new();
}

public sealed class TeamScheduleStatusTypeDto
{
    public string Name { get; init; } = string.Empty;
    public string? ShortDetail { get; init; }
    public string? Detail { get; init; }
}