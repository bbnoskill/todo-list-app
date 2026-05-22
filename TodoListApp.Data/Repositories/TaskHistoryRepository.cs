using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;
using TodoListApp.Data.Interfaces;

namespace TodoListApp.Data.Repositories;

/// <summary>
/// Repository for managing task history entities.
/// </summary>
public class TaskHistoryRepository : ITaskHistoryRepository
{
    private readonly TodoListDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskHistoryRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TaskHistoryRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task AddAsync(TaskHistoryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await this.context.TaskHistories.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TaskHistoryEntity>> GetRecentByUserIdAsync(string userId)
    {
        return await this.context.TaskHistories
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.ActionDate)
            .Take(10)
            .ToListAsync();
    }
}
