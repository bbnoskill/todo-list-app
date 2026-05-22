namespace TodoListApp.WebApi.Models;

/// <summary>
/// Model for a task history record in API responses.
/// </summary>
public class TaskHistoryModel
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task that was acted upon.
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of action performed.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time the action was performed.
    /// </summary>
    public DateTime ActionDate { get; set; }
}
