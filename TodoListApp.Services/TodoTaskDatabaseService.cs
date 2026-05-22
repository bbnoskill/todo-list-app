using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;
using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Service for managing to-do tasks in the database.
/// </summary>
public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly ITodoTaskRepository repository;
    private readonly ITaskHistoryRepository historyRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskDatabaseService"/> class.
    /// </summary>
    /// <param name="repository">The to-do task repository.</param>
    /// <param name="historyRepository">The task history repository.</param>
    public TodoTaskDatabaseService(ITodoTaskRepository repository, ITaskHistoryRepository historyRepository)
    {
        this.repository = repository;
        this.historyRepository = historyRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoTask>> GetTodoTasksAsync(string userId)
    {
        var entities = await this.repository.GetByUserIdAsync(userId);
        return entities.Select(e => new TodoTask
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            DueDate = e.DueDate,
            CreatedDate = e.CreatedDate,
            IsCompleted = e.IsCompleted,
            TodoListId = e.TodoListId,
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoTask>> GetAssignedTodoTasksAsync(string userId, bool? isCompleted, string? sortBy)
    {
        var entities = await this.repository.GetByUserIdAsync(userId);
        if (isCompleted is not null)
        {
            entities = entities.Where(e => e.IsCompleted == isCompleted);
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            entities = sortBy.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
            {
                "title" => entities.OrderBy(e => e.Title),
                "duedate" => entities.OrderBy(e => e.DueDate),
                _ => entities
            };
        }

        return entities.Select(e => new TodoTask
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            DueDate = e.DueDate,
            CreatedDate = e.CreatedDate,
            IsCompleted = e.IsCompleted,
            TodoListId = e.TodoListId,
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoTask>> SearchTodoTasksAsync(string userId, string? searchQuery, string? searchBy)
    {
        var entities = await this.repository.GetByUserIdAsync(userId);
        var query = entities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery) && !string.IsNullOrWhiteSpace(searchBy))
        {
            switch (searchBy.ToUpperInvariant())
            {
                case "TITLE":
                    query = query.Where(t => t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                    break;
                case "CREATEDDATE":
                    if (DateTime.TryParse(searchQuery, out var parsedCreatedDate))
                    {
                        query = query.Where(t => t.CreatedDate.Date == parsedCreatedDate.Date);
                    }

                    break;
                case "DUEDATE":
                    if (DateTime.TryParse(searchQuery, out var parsedDueDate))
                    {
                        query = query.Where(t => t.DueDate.Date == parsedDueDate.Date);
                    }

                    break;
            }
        }

        return query.Select(t => new TodoTask
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

    /// <inheritdoc/>
    public async Task<TodoTask?> GetTodoTaskByIdAsync(int id, string userId)
    {
        var entity = await this.repository.GetByIdWithTagsAsync(id);

        if (entity == null || entity.TodoList.UserId != userId)
        {
            return null;
        }

        return new TodoTask
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueDate = entity.DueDate,
            CreatedDate = entity.CreatedDate,
            IsCompleted = entity.IsCompleted,
            TodoListId = entity.TodoListId,
        };
    }

    /// <inheritdoc/>
    public Task AddTodoTaskAsync(TodoTask todoTask)
    {
        ArgumentNullException.ThrowIfNull(todoTask);
        return this.AddTodoTaskAsyncCore(todoTask);
    }

    /// <inheritdoc/>
    public Task UpdateTodoTaskAsync(TodoTask todoTask)
    {
        ArgumentNullException.ThrowIfNull(todoTask);
        return this.UpdateTodoTaskAsyncCore(todoTask);
    }

    /// <inheritdoc/>
    public async Task ChangeTaskStatusAsync(int id, bool isCompleted, string userId)
    {
        var entity = await this.repository.GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsCompleted = isCompleted;
            await this.repository.UpdateAsync(entity);

            if (isCompleted)
            {
                var historyEntry = new TaskHistoryEntity
                {
                    UserId = userId,
                    TaskTitle = entity.Title,
                    Action = "Completed",
                    ActionDate = DateTime.Now,
                };

                await this.historyRepository.AddAsync(historyEntry);
            }
        }
    }

    /// <inheritdoc/>
    public async Task DeleteTodoTaskAsync(int id, string userId)
    {
        var entity = await this.repository.GetByIdAsync(id);
        if (entity != null)
        {
            var historyEntry = new TaskHistoryEntity
            {
                UserId = userId,
                TaskTitle = entity.Title,
                Action = "Deleted",
                ActionDate = DateTime.Now,
            };

            await this.historyRepository.AddAsync(historyEntry);
            await this.repository.DeleteAsync(entity);
        }
    }

    private async Task AddTodoTaskAsyncCore(TodoTask todoTask)
    {
        var entity = new TodoTaskEntity
        {
            Title = todoTask.Title,
            Description = todoTask.Description,
            DueDate = todoTask.DueDate,
            CreatedDate = DateTime.Now,
            IsCompleted = todoTask.IsCompleted,
            TodoListId = todoTask.TodoListId,
        };

        await this.repository.AddAsync(entity);
        todoTask.Id = entity.Id;
    }

    private async Task UpdateTodoTaskAsyncCore(TodoTask todoTask)
    {
        var entity = await this.repository.GetByIdAsync(todoTask.Id);
        if (entity != null)
        {
            entity.Title = todoTask.Title;
            entity.Description = todoTask.Description;
            entity.DueDate = todoTask.DueDate;
            entity.IsCompleted = todoTask.IsCompleted;
            entity.TodoListId = todoTask.TodoListId;
            await this.repository.UpdateAsync(entity);
        }
    }
}
