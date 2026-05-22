using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;
using TodoListApp.Domain;

namespace TodoListApp.Services;

public class TagDatabaseService : ITagDatabaseService
{
    private readonly TodoListDbContext dbContext;
    private readonly ITodoTaskRepository taskRepository;

    public TagDatabaseService(TodoListDbContext dbContext, ITodoTaskRepository taskRepository)
    {
        this.dbContext = dbContext;
        this.taskRepository = taskRepository;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        var tags = await this.dbContext.Tags.ToListAsync();
        return tags.Select(t => new Tag { Id = t.Id, Name = t.Name });
    }

    public async Task<IEnumerable<Tag>> GetTagsForTaskAsync(int taskId)
    {
        var task = await this.taskRepository.GetByIdWithTagsAsync(taskId);
        if (task == null)
        {
            return Enumerable.Empty<Tag>();
        }

        return task.Tags.Select(t => new Tag { Id = t.Id, Name = t.Name });
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByTagAsync(int tagId)
    {
        var tag = await this.dbContext.Tags
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == tagId);
        if (tag == null)
        {
            return Enumerable.Empty<TodoTask>();
        }

        return tag.Tasks.Select(t => new TodoTask
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
        var task = await this.taskRepository.GetByIdWithTagsAsync(taskId);
        if (task == null)
        {
            return;
        }

        var existingTag = await this.dbContext.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        if (existingTag == null)
        {
            existingTag = new TagEntity { Name = tagName };
            this.dbContext.Tags.Add(existingTag);
        }

        if (task.Tags.All(t => t.Name != tagName))
        {
            task.Tags.Add(existingTag);
            await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var task = await this.taskRepository.GetByIdWithTagsAsync(taskId);
        if (task == null)
        {
            return;
        }

        var tagToRemove = task.Tags.FirstOrDefault(t => t.Id == tagId);
        if (tagToRemove != null)
        {
            task.Tags.Remove(tagToRemove);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
