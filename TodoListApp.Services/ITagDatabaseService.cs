using TodoListApp.Domain;

namespace TodoListApp.Services;

public interface ITagDatabaseService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();

    Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId);

    Task<IEnumerable<TodoTask>> GetTasksByTagAsync(int tagId, string userId);

    Task AddTagToTaskAsync(int taskId, string tagName);

    Task RemoveTagFromTaskAsync(int taskId, int tagId);
}
