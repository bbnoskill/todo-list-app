using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpclient;

    public TagWebApiService(HttpClient httpclient)
    {
        this.httpclient = httpclient;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        var tags = await this.httpclient.GetFromJsonAsync<IEnumerable<Tag>>("Tag");
        if (tags == null)
        {
            return Enumerable.Empty<Tag>();
        }

        return tags.Select(t => new Tag { Id = t.Id, Name = t.Name, });
    }

    public async Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId)
    {
        var tags = await this.httpclient.GetFromJsonAsync<IEnumerable<Tag>>($"Tag/{taskId}/tags");
        if (tags == null)
        {
            return Enumerable.Empty<Tag>();
        }

        return tags.Select(t => new Tag { Id = t.Id, Name = t.Name, });
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByTagAsync(int tagId)
    {
        var tags = await this.httpclient.GetFromJsonAsync<IEnumerable<TodoTaskWebApiModel>>($"Tag/{tagId}/tasks");
        if (tags == null)
        {
            return Enumerable.Empty<TodoTask>();
        }

        return tags.Select(t => new TodoTask
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            CreatedDate = t.CreatedDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        });
    }

    public async Task AddTagToTaskAsync(int taskId, string tagName)
    {
        var response = await this.httpclient.PostAsJsonAsync($"Tag/{taskId}/tags", tagName);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var response = await this.httpclient.DeleteAsync(new Uri($"Tag/{taskId}/tags/{tagId}", UriKind.Relative));
        response.EnsureSuccessStatusCode();
    }
}
