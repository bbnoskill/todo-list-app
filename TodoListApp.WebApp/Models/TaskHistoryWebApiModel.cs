namespace TodoListApp.WebApp.Models;

/// <summary>
/// Model for a task history record received from the Web API.
/// </summary>
public class TaskHistoryWebApiModel
{
    public int Id { get; set; }

    public string TaskTitle { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public DateTime ActionDate { get; set; }
}
