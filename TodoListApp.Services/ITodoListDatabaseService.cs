using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Interface for managing to-do lists in the database.
/// </summary>
public interface ITodoListDatabaseService
{
    /// <summary>
    /// Gets all to-do lists for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of to-do lists.</returns>
    Task<IEnumerable<TodoList>> GetTodoListsAsync(string userId);

    /// <summary>
    /// Gets a to-do list by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The to-do list.</returns>
    Task<TodoList?> GetTodoListByIdAsync(int id, string userId);

    /// <summary>
    /// Adds a to-do list for a specific user.
    /// </summary>
    /// <param name="todoList">The to-do list to add.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddTodoListAsync(TodoList todoList, string userId);

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list with updated property values.</param>
    Task UpdateTodoListAsync(TodoList todoList);

    /// <summary>
    /// Deletes a to-do list by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the to-do list to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteTodoListAsync(int id);
}
