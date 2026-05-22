using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TodoTaskWebApiService : ITodoTaskWebApiService
{
    private readonly HttpClient httpclient;

    public TodoTaskWebApiService(HttpClient httpClient)
    {
        this.httpclient = httpClient;
    }

    public async Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int todoListId)
    {
        var models =
            await this.httpclient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>($"TodoList/{todoListId}/tasks");
        if (models == null)
        {
            return Enumerable.Empty<TodoTask>();
        }

        return models.Select(t => new TodoTask
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        });
    }

    public async Task<TodoTask?> GetTodoTaskByIdAsync(int id)
    {
        var model = await this.httpclient.GetFromJsonAsync<TodoTaskWebApiModel>($"Tasks/{id}");
        if (model == null)
        {
            return null;
        }

        return new TodoTask
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            IsCompleted = model.IsCompleted,
            TodoListId = model.TodoListId,
        };
    }

    public async Task<IEnumerable<TodoTask>> GetAssignedTodoTasksAsync(bool? isCompleted, string? sortBy)
    {
        var queryParams = new List<string>();
        if (isCompleted.HasValue)
        {
            queryParams.Add($"isCompleted={isCompleted.Value}");
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            queryParams.Add($"sortBy={sortBy}");
        }

        var queryString = queryParams.Count != 0 ? "?" + string.Join("&", queryParams) : string.Empty;

        var models = await this.httpclient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>($"Tasks/assigned{queryString}");
        if (models == null)
        {
            return Enumerable.Empty<TodoTask>();
        }

        return models.Select(m => new TodoTask
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            DueDate = m.DueDate,
            IsCompleted = m.IsCompleted,
            TodoListId = m.TodoListId,
        });
    }

    public async Task<IEnumerable<TodoTask>> SearchTodoTasksAsync(string searchQuery, string searchBy)
    {
        if (string.IsNullOrWhiteSpace(searchQuery) || string.IsNullOrWhiteSpace(searchBy))
        {
            return Enumerable.Empty<TodoTask>();
        }

        var url = $"Tasks/search?searchQuery={Uri.EscapeDataString(searchQuery)}&searchBy={Uri.EscapeDataString(searchBy)}";
        var models = await this.httpclient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>(url);
        if (models == null)
        {
            return Enumerable.Empty<TodoTask>();
        }

        return models.Select(m => new TodoTask
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            DueDate = m.DueDate,
            CreatedDate = m.CreatedDate,
            IsCompleted = m.IsCompleted,
            TodoListId = m.TodoListId,
        });
    }

    public Task AddTodoTaskAsync(TodoTask todoTask)
    {
        ArgumentNullException.ThrowIfNull(todoTask);
        return this.AddTodoTaskAsyncCore(todoTask);
    }

    public Task UpdateTodoTaskAsync(TodoTask todoTask)
    {
        ArgumentNullException.ThrowIfNull(todoTask);
        return this.UpdateTodoTaskAsyncCore(todoTask);
    }

    public async Task ChangeTaskStatusAsync(int id, bool isCompleted)
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, $"Tasks/{id}/status");
        request.Content = JsonContent.Create(isCompleted);
        var response = await this.httpclient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTodoTaskAsync(int id)
    {
        var response = await this.httpclient.DeleteAsync(new Uri($"Tasks/{id}", UriKind.Relative));
        response.EnsureSuccessStatusCode();
    }

    private async Task AddTodoTaskAsyncCore(TodoTask todoTask)
    {
        var model = new TodoTaskWebApiModel
        {
            Title = todoTask.Title,
            Description = todoTask.Description,
            DueDate = todoTask.DueDate,
            IsCompleted = todoTask.IsCompleted,
            TodoListId = todoTask.TodoListId,
        };
        var response = await this.httpclient.PostAsJsonAsync($"TodoList/{todoTask.TodoListId}/tasks", model);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateTodoTaskAsyncCore(TodoTask todoTask)
    {
        var model = new TodoTaskWebApiModel
        {
            Title = todoTask.Title,
            Description = todoTask.Description,
            DueDate = todoTask.DueDate,
            IsCompleted = todoTask.IsCompleted,
            TodoListId = todoTask.TodoListId,
        };
        var response = await this.httpclient.PutAsJsonAsync($"Tasks/{todoTask.Id}", model);
        response.EnsureSuccessStatusCode();
    }
}
