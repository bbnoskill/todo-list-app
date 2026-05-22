using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;
using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Service for managing to-do lists in the database.
/// </summary>
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly ITodoListRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDatabaseService"/> class.
    /// </summary>
    /// <param name="repository">The to-do list repository.</param>
    public TodoListDatabaseService(ITodoListRepository repository)
    {
        this.repository = repository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoList>> GetTodoListsAsync(string userId)
    {
        var entities = await this.repository.GetByUserIdAsync(userId);
        return entities.Select(e => new TodoList
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
        });
    }

    /// <inheritdoc/>
    public async Task<TodoList?> GetTodoListByIdAsync(int id, string userId)
    {
        var entity = await this.repository.GetByIdAsync(id);
        if (entity == null || entity.UserId != userId)
        {
            return null;
        }

        return new TodoList
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
        };
    }

    /// <inheritdoc/>
    public Task AddTodoListAsync(TodoList todoList, string userId)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        return this.AddTodoListAsyncCore(todoList, userId);
    }

    /// <inheritdoc/>
    public Task UpdateTodoListAsync(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);
        return this.UpdateTodoListAsyncCore(todoList);
    }

    /// <inheritdoc/>
    public async Task DeleteTodoListAsync(int id)
    {
        var entity = await this.repository.GetByIdAsync(id);
        if (entity != null)
        {
            await this.repository.DeleteAsync(entity);
        }
    }

    private async Task AddTodoListAsyncCore(TodoList todoList, string userId)
    {
        var entity = new TodoListEntity
        {
            Title = todoList.Title,
            Description = todoList.Description,
            UserId = userId,
        };

        await this.repository.AddAsync(entity);
        todoList.Id = entity.Id;
    }

    private async Task UpdateTodoListAsyncCore(TodoList todoList)
    {
        var entity = await this.repository.GetByIdAsync(todoList.Id);
        if (entity != null)
        {
            entity.Title = todoList.Title;
            entity.Description = todoList.Description;
            await this.repository.UpdateAsync(entity);
        }
    }
}
