namespace TaskTrackerAPI.Models;

// ── DTOs (Records — modern C# feature) ───────────────────────────────────────

public record CreateBugRequest(string Title, SeverityLevel SeverityLevel);

public record CreateFeatureRequest(string Title, double EstimatedHours);

public record TaskResponse(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    bool IsCompleted,
    string Type,
    // nullable extras
    SeverityLevel? SeverityLevel = null,
    double? EstimatedHours = null
);
