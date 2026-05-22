using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoListWebApiService
{
    /// <summary>
    /// Gets all to-do lists.
    /// </summary>
    /// <returns>A collection of to-do lists.</returns>
    Task<IEnumerable<TodoList>> GetTodoListsAsync();

    /// <summary>
    /// Gets a to-do list by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The to-do list.</returns>
    Task<TodoList?> GetTodoListByIdAsync(int id);

    /// <summary>
    /// Adds a new to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddTodoListAsync(TodoList todoList);

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list with updated property values.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateTodoListAsync(TodoList todoList);

    /// <summary>
    /// Deletes a to-do list by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the to-do list to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteTodoListAsync(int id);
}
