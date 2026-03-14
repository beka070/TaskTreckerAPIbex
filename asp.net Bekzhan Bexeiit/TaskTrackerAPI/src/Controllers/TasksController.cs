using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Models;
using TaskTrackerAPI.Repository;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers;

// ── Block 2 — Web API controller ─────────────────────────────────────────────

[ApiController]
[Route("api/tasks")]
public class TasksController(ITaskRepository repo, ILogger<TasksController> logger) : ControllerBase
{
    // GET /api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskResponse>> GetAll() =>
        Ok(repo.GetAll().Select(ToResponse));

    // GET /api/tasks/stats
    [HttpGet("stats")]
    public ActionResult<object> GetStats()
    {
        var all = repo.GetAll();
        return Ok(new
        {
            HighSeverityBugs = TaskFilterService.GetHighSeverityBugs(all).Select(ToResponse),
            TotalEstimatedHours = TaskFilterService.GetTotalEstimatedHours(all)
        });
    }

    // POST /api/tasks/bug
    [HttpPost("bug")]
    public ActionResult<TaskResponse> CreateBug([FromBody] CreateBugRequest req)
    {
        var task = new BugReportTask
        {
            Title = req.Title,
            SeverityLevel = req.SeverityLevel
        };

        // subscribe to the event (Block 1.4)
        task.OnTaskCompleted += t =>
            logger.LogInformation("BugReportTask '{Title}' (id={Id}) completed.", t.Title, t.Id);

        repo.Add(task);
        return CreatedAtAction(nameof(GetAll), ToResponse(task));
    }

    // POST /api/tasks/feature
    [HttpPost("feature")]
    public ActionResult<TaskResponse> CreateFeature([FromBody] CreateFeatureRequest req)
    {
        var task = new FeatureRequestTask
        {
            Title = req.Title,
            EstimatedHours = req.EstimatedHours
        };

        task.OnTaskCompleted += t =>
            logger.LogInformation("FeatureRequestTask '{Title}' (id={Id}) completed.", t.Title, t.Id);

        repo.Add(task);
        return CreatedAtAction(nameof(GetAll), ToResponse(task));
    }

    // PUT /api/tasks/{id}/complete
    [HttpPut("{id:guid}/complete")]
    public ActionResult Complete(Guid id)
    {
        var task = repo.GetById(id);

        // pattern matching (modern C# feature)
        return task switch
        {
            null => NotFound(new { message = $"Task {id} not found." }),
            { IsCompleted: true } => Conflict(new { message = "Task is already completed." }),
            _ => ExecuteComplete(task)
        };
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private OkResult ExecuteComplete(BaseTask task)
    {
        task.CompleteTask(); // fires OnTaskCompleted event
        return Ok();
    }

    private static TaskResponse ToResponse(BaseTask t) => t switch
    {
        BugReportTask bug     => new TaskResponse(bug.Id, bug.Title, bug.CreatedAt, bug.IsCompleted, "BugReport", SeverityLevel: bug.SeverityLevel),
        FeatureRequestTask fr => new TaskResponse(fr.Id, fr.Title, fr.CreatedAt, fr.IsCompleted, "FeatureRequest", EstimatedHours: fr.EstimatedHours),
        _                     => new TaskResponse(t.Id, t.Title, t.CreatedAt, t.IsCompleted, "Unknown")
    };
}
