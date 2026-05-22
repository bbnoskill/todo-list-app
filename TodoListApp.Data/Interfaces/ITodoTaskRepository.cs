using TodoListApp.Data.Entities;

namespace TodoListApp.Data.Interfaces;

/// <summary>
/// Repository interface for to-do lists.
/// </summary>
public interface ITodoTaskRepository
{
    /// <summary>
    /// Gets all tasks for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of task entities.</returns>
    Task<IEnumerable<TodoTaskEntity>> GetByUserIdAsync(string userId);

    /// <summary>
    /// Gets all to-do lists.
    /// </summary>
    /// <returns>A collection of to-do list entities.</returns>
    Task<IEnumerable<TodoTaskEntity>> GetAllAsync();

    /// <summary>
    /// Gets a to-do list by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The to-do list entity.</returns>
    Task<TodoTaskEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a to-do task by id including its tags.
    /// </summary>
    Task<TodoTaskEntity?> GetByIdWithTagsAsync(int id);

    /// <summary>
    /// Adds a to-do list.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TodoTaskEntity entity);

    /// <summary>
    /// Updates a to-do list.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(TodoTaskEntity entity);

    /// <summary>
    /// Deletes a to-do list.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(TodoTaskEntity entity);
}
