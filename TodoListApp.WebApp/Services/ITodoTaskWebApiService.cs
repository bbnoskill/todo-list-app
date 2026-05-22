using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITodoTaskWebApiService
{
        Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int todoListId);

        Task<IEnumerable<TodoTask>> GetAssignedTodoTasksAsync(bool? isCompleted, string? sortBy);

        Task<TodoTask?> GetTodoTaskByIdAsync(int id);
        Task<IEnumerable<TodoTask>> SearchTodoTasksAsync(string searchQuery, string searchBy);

        Task AddTodoTaskAsync(TodoTask todoTask);

        Task UpdateTodoTaskAsync(TodoTask todoTask);

        Task ChangeTaskStatusAsync(int id, bool isCompleted);

        Task DeleteTodoTaskAsync(int id);
}
