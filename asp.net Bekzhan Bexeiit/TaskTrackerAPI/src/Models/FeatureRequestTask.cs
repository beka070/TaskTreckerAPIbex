namespace TaskTrackerAPI.Models;

/// <summary>A task that represents a feature request.</summary>
public class FeatureRequestTask : BaseTask
{
    public double EstimatedHours { get; set; }
}
