using TodoListApp.Data.Interfaces;
using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Service for managing task history in the database.
/// </summary>
public class TaskHistoryDatabaseService : ITaskHistoryDatabaseService
{
    private readonly ITaskHistoryRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskHistoryDatabaseService"/> class.
    /// </summary>
    /// <param name="repository">The task history repository.</param>
    public TaskHistoryDatabaseService(ITaskHistoryRepository repository)
    {
        this.repository = repository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TaskHistory>> GetRecentHistoryAsync(string userId)
    {
        var entities = await this.repository.GetRecentByUserIdAsync(userId);
        return entities.Select(e => new TaskHistory
        {
            Id = e.Id,
            UserId = e.UserId,
            TaskTitle = e.TaskTitle,
            Action = e.Action,
            ActionDate = e.ActionDate,
        });
    }
}
