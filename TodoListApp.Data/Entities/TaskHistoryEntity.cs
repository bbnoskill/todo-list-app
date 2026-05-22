namespace TodoListApp.Data.Entities;

/// <summary>
/// Entity representing a task history record in the database.
/// </summary>
public class TaskHistoryEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the history record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who performed the action.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title of the task that was acted upon.
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of action performed (Completed or Deleted).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time the action was performed.
    /// </summary>
    public DateTime ActionDate { get; set; }
}
