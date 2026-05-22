using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;

namespace TodoListApp.Data.Repositories;

/// <summary>
/// Repository for managing to-do list entities.
/// </summary>
public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TodoListRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoListEntity>> GetByUserIdAsync(string userId)
    {
        return await this.context.TodoLists
            .Where(l => l.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TodoListEntity>> GetAllAsync()
    {
        return await this.context.TodoLists.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TodoListEntity?> GetByIdAsync(int id)
    {
        return await this.context.TodoLists.FindAsync(id);
    }

    /// <inheritdoc/>
    public Task AddAsync(TodoListEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.AddAsyncCore(entity);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TodoListEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.UpdateAsyncCore(entity);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(TodoListEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return this.DeleteAsyncCore(entity);
    }

    private async Task AddAsyncCore(TodoListEntity entity)
    {
        await this.context.TodoLists.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    private async Task UpdateAsyncCore(TodoListEntity entity)
    {
        this.context.TodoLists.Update(entity);
        await this.context.SaveChangesAsync();
    }

    private async Task DeleteAsyncCore(TodoListEntity entity)
    {
        this.context.TodoLists.Remove(entity);
        await this.context.SaveChangesAsync();
    }
}
