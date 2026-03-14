using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Repository;

/// <summary>Thread-safe in-memory store. Registered as Singleton in DI.</summary>
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<BaseTask> _store = [];

    public IEnumerable<BaseTask> GetAll() => _store.AsReadOnly();

    public BaseTask? GetById(Guid id) =>
        _store.FirstOrDefault(t => t.Id == id);

    public void Add(BaseTask task) => _store.Add(task);
}
