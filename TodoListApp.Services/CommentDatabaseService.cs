using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Data.Entities;
using TodoListApp.Domain;

namespace TodoListApp.Services;

public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly TodoListDbContext dbContext;

    public CommentDatabaseService(TodoListDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Comment>> GetCommentsForTaskAsync(int taskId)
    {
        var entities = await this.dbContext.Comments
            .Where(c => c.TodoTaskId == taskId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        return entities.Select(e => new Comment
        {
            Id = e.Id,
            Text = e.Text,
            CreatedDate = e.CreatedDate,
            TodoTaskId = e.TodoTaskId,
        });
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        var entity = await this.dbContext.Comments.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        return new Comment
        {
            Id = entity.Id,
            Text = entity.Text,
            CreatedDate = entity.CreatedDate,
            TodoTaskId = entity.TodoTaskId,
        };
    }

    public Task AddCommentAsync(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        return this.AddCommentAsyncCore(comment);
    }

    public Task UpdateCommentAsync(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        return this.UpdateCommentAsyncCore(comment);
    }

    public async Task DeleteCommentAsync(int id)
    {
        var entity = await this.dbContext.Comments.FindAsync(id);
        if (entity != null)
        {
            this.dbContext.Comments.Remove(entity);
            await this.dbContext.SaveChangesAsync();
        }
    }

    private async Task AddCommentAsyncCore(Comment comment)
    {
        var entity = new CommentEntity
        {
            Text = comment.Text,
            CreatedDate = DateTime.Now,
            TodoTaskId = comment.TodoTaskId,
        };

        await this.dbContext.Comments.AddAsync(entity);
        await this.dbContext.SaveChangesAsync();

        comment.Id = entity.Id;
        comment.CreatedDate = entity.CreatedDate;
    }

    private async Task UpdateCommentAsyncCore(Comment comment)
    {
        var entity = await this.dbContext.Comments.FindAsync(comment.Id);
        if (entity != null)
        {
            entity.Text = comment.Text;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
