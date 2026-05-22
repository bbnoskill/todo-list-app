using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Service interface for managing task history.
/// </summary>
public interface ITaskHistoryDatabaseService
{
    /// <summary>
    /// Gets the last 10 history records for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of task history records.</returns>
    Task<IEnumerable<TaskHistory>> GetRecentHistoryAsync(string userId);
}
