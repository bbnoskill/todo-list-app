using TodoListApp.Data.Entities;

namespace TodoListApp.Data.Interfaces;

/// <summary>
/// Repository interface for to-do lists.
/// </summary>
public interface ITodoListRepository
{
    /// <summary>
    /// Gets all to-do lists for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of to-do list entities.</returns>
    Task<IEnumerable<TodoListEntity>> GetByUserIdAsync(string userId);

    /// <summary>
    /// Gets all to-do lists.
    /// </summary>
    /// <returns>A collection of to-do list entities.</returns>
    Task<IEnumerable<TodoListEntity>> GetAllAsync();

    /// <summary>
    /// Gets a to-do list by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The to-do list entity.</returns>
    Task<TodoListEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a to-do list.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TodoListEntity entity);

    /// <summary>
    /// Updates a to-do list.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(TodoListEntity entity);

    /// <summary>
    /// Deletes a to-do list.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(TodoListEntity entity);
}
