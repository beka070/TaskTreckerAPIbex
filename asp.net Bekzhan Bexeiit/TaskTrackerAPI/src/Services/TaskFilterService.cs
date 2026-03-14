using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Services;

// ── Block 1.5 — static filter service using LINQ ─────────────────────────────

public static class TaskFilterService
{
    /// <summary>
    /// Returns all incomplete BugReportTasks with High (or Critical) severity,
    /// sorted by CreatedAt descending (newest first).
    /// </summary>
    public static IEnumerable<BugReportTask> GetHighSeverityBugs(IEnumerable<BaseTask> tasks) =>
        tasks
            .OfType<BugReportTask>()
            .Where(t => !t.IsCompleted && t.SeverityLevel >= SeverityLevel.High)
            .OrderByDescending(t => t.CreatedAt);

    /// <summary>
    /// Returns the total estimated hours for all incomplete FeatureRequestTasks.
    /// </summary>
    public static double GetTotalEstimatedHours(IEnumerable<BaseTask> tasks) =>
        tasks
            .OfType<FeatureRequestTask>()
            .Where(t => !t.IsCompleted)
            .Sum(t => t.EstimatedHours);
}
