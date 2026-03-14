namespace TaskTrackerAPI.Models;

// ── Block 1.3 ────────────────────────────────────────────────────────────────

public enum SeverityLevel { Low, Medium, High, Critical }

/// <summary>A task that represents a bug report.</summary>
public class BugReportTask : BaseTask
{
    public SeverityLevel SeverityLevel { get; set; }
}
