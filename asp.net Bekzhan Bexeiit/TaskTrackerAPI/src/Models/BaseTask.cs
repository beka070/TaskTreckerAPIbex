namespace TaskTrackerAPI.Models;

/// <summary>
/// Abstract base class for all task types.
/// Id and CreatedAt are init-only — set once at construction (encapsulation).
/// </summary>
public abstract class BaseTask
{
    // ── Block 1.1 / 1.2 ──────────────────────────────────────────────────────
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public bool IsCompleted { get; private set; }

    // ── Block 1.4 — delegate + event ─────────────────────────────────────────
    public delegate void TaskCompletedHandler(BaseTask task);
    public event TaskCompletedHandler? OnTaskCompleted;

    /// <summary>Marks the task as completed and fires the event.</summary>
    public void CompleteTask()
    {
        if (IsCompleted) return;
        IsCompleted = true;
        OnTaskCompleted?.Invoke(this);
    }
}
