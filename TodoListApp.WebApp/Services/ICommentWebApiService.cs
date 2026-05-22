using TodoListApp.WebApp.Models;
namespace TodoListApp.WebApp.Services;

public interface ICommentWebApiService
{
    Task<IEnumerable<Comment>> GetCommentsForTaskAsync(int taskId);
    Task<Comment?> GetCommentByIdAsync(int id);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
}
