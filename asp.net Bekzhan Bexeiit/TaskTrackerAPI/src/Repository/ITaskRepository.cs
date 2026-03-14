using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Repository;

// ── Block 2 — repository abstraction (DI-friendly) ───────────────────────────

public interface ITaskRepository
{
    IEnumerable<BaseTask> GetAll();
    BaseTask? GetById(Guid id);
    void Add(BaseTask task);
}
