using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;

namespace TodoListApp.Data.Repositories;

/// <summary>
/// Repository for managing to-do task entities.
/// </summary>
public class TodoTaskRepository : ITodoTaskRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TodoTaskRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoTaskEntity>> GetByUserIdAsync(string userId)
    {
        return await this.context.TodoTasks
            .Include(t => t.TodoList)
            .Where(t => t.TodoList.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoTaskEntity>> GetAllAsync()
    {
        return await this.context.TodoTasks.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TodoTaskEntity?> GetByIdAsync(int id)
    {
        return await this.context.TodoTasks.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<TodoTaskEntity?> GetByIdAsync(int id, string userId)
    {
        return await this.context.TodoTasks
            .Include(t => t.TodoList)
            .FirstOrDefaultAsync(t => t.Id == id && t.TodoList.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<TodoTaskEntity?> GetByIdWithTagsAsync(int id)
    {
        return await this.context.TodoTasks
            .Include(t => t.Tags)
            .Include(t => t.TodoList)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public Task AddAsync(TodoTaskEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.AddAsyncCore(entity);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TodoTaskEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.UpdateAsyncCore(entity);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(TodoTaskEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.DeleteAsyncCore(entity);
    }

    private async Task AddAsyncCore(TodoTaskEntity entity)
    {
        await this.context.TodoTasks.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    private async Task UpdateAsyncCore(TodoTaskEntity entity)
    {
        this.context.TodoTasks.Update(entity);
        await this.context.SaveChangesAsync();
    }

    private async Task DeleteAsyncCore(TodoTaskEntity entity)
    {
        this.context.TodoTasks.Remove(entity);
        await this.context.SaveChangesAsync();
    }
}
