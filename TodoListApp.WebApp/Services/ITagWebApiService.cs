using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface ITagWebApiService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId);
    Task<IEnumerable<TodoTask>> GetTasksByTagAsync(int tagId);
    Task AddTagToTaskAsync(int taskId, string tagName);
    Task RemoveTagFromTaskAsync(int taskId, int tagId);
}
