using TodoListApp.Domain;

namespace TodoListApp.Services;

/// <summary>
/// Provides operations for managing comments.
/// </summary>
public interface ICommentDatabaseService
{
    Task<IEnumerable<Comment>> GetCommentsForTaskAsync(int taskId);

    Task<Comment?> GetCommentByIdAsync(int id);

    Task AddCommentAsync(Comment comment);

    Task UpdateCommentAsync(Comment comment);

    Task DeleteCommentAsync(int id);
}
