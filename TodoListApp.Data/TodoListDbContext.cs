using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;

namespace TodoListApp.Data;

/// <summary>
/// Database context for the TodoList application.
/// </summary>
public class TodoListDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the to-do lists.
    /// </summary>
    public DbSet<TodoListEntity> TodoLists { get; set; } = null!;

    public DbSet<TodoTaskEntity> TodoTasks { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    public DbSet<TaskHistoryEntity> TaskHistories { get; set; } = null!;
}
