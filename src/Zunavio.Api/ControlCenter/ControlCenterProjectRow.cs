namespace Zunavio.Api.ControlCenter;

public sealed record ControlCenterProjectRow
{
    public string ProjectId { get; init; } = string.Empty;
    public string WorkingTitle { get; init; } = string.Empty;
    public string FinalTitle { get; init; } = string.Empty;
    public string Marketplace { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public string TargetAge { get; init; } = string.Empty;
    public string BookType { get; init; } = string.Empty;
    public string Season { get; init; } = string.Empty;
    public string CurrentGate { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string MarketScore { get; init; } = string.Empty;
    public string ManuscriptVersion { get; init; } = string.Empty;
    public string VisualBibleVersion { get; init; } = string.Empty;
    public string ProductionVersion { get; init; } = string.Empty;
    public string QaResult { get; init; } = string.Empty;
    public string NextAction { get; init; } = string.Empty;
    public string CreatedAt { get; init; } = string.Empty;
    public string UpdatedAt { get; init; } = string.Empty;
    public string ProjectFolderUrl { get; init; } = string.Empty;
}
