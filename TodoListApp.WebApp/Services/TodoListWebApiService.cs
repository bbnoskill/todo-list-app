using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
    {
        var models = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoListWebApiModel>>("TodoList");
        if (models == null)
        {
            return Enumerable.Empty<TodoList>();
        }

        return models.Select(m => new TodoList
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
        });
    }

    public async Task<TodoList?> GetTodoListByIdAsync(int id)
    {
        var model = await this.httpClient.GetFromJsonAsync<TodoListWebApiModel>($"TodoList/{id}");
        if (model == null)
        {
            return null;
        }

        return new TodoList()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
        };
    }

    public Task AddTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        return this.AddTodoListAsyncCore(todoList);
    }

    public async Task DeleteTodoListAsync(int id)
    {
        await this.httpClient.DeleteAsync(new Uri($"TodoList/{id}", UriKind.Relative));
    }

    public Task UpdateTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        return this.UpdateTodoListAsyncCore(todoList);
    }

    private async Task AddTodoListAsyncCore(TodoList todoList)
    {
        var model = new TodoListWebApiModel
        {
            Title = todoList.Title,
            Description = todoList.Description,
        };

        await this.httpClient.PostAsJsonAsync("TodoList", model);
    }

    private async Task UpdateTodoListAsyncCore(TodoList todoList)
    {
        var model = new TodoListWebApiModel
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,
        };

        await this.httpClient.PutAsJsonAsync($"TodoList/{todoList.Id}", model);
    }
}
