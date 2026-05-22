using TodoListApp.Data.Entities;

namespace TodoListApp.Data.Interfaces;

/// <summary>
/// Repository interface for task history records.
/// </summary>
public interface ITaskHistoryRepository
{
    /// <summary>
    /// Adds a task history record.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TaskHistoryEntity entity);

    /// <summary>
    /// Gets the last 10 history records for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of task history entities.</returns>
    Task<IEnumerable<TaskHistoryEntity>> GetRecentByUserIdAsync(string userId);
}
