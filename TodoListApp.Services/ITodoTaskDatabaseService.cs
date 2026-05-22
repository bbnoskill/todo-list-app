using TodoListApp.Domain;

namespace TodoListApp.Services;

public interface ITodoTaskDatabaseService
{
    Task<IEnumerable<TodoTask>> GetTodoTasksAsync(string userId);

    Task<IEnumerable<TodoTask>> GetAssignedTodoTasksAsync(string userId, bool? isCompleted, string? sortBy);

    Task<TodoTask?> GetTodoTaskByIdAsync(int id, string userId);

    Task AddTodoTaskAsync(TodoTask todoTask);

    Task UpdateTodoTaskAsync(TodoTask todoTask);

    Task ChangeTaskStatusAsync(int id, bool isCompleted, string userId);

    Task DeleteTodoTaskAsync(int id, string userId);

    Task<IEnumerable<TodoTask>> SearchTodoTasksAsync(string userId, string? searchQuery, string? searchBy);
}
