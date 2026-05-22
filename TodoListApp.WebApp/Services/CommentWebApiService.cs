using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpclient;

    public CommentWebApiService(HttpClient httpclient)
    {
        this.httpclient = httpclient;
    }

    public async Task<IEnumerable<Comment>> GetCommentsForTaskAsync(int taskId)
    {
        var models = await this.httpclient.GetFromJsonAsync<IEnumerable<CommentWebApiModel>>($"Comments/task/{taskId}");
        if (models == null)
        {
            return new List<Comment>();
        }

        return models.Select(m => new Comment
        {
            Id = m.Id,
            Text = m.Text,
            CreatedDate = m.CreatedDate,
            TodoTaskId = m.TodoTaskId,
        });
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        var model = await this.httpclient.GetFromJsonAsync<CommentWebApiModel>($"Comments/{id}");
        if (model == null)
        {
            return null;
        }

        return new Comment
        {
            Id = model.Id,
            Text = model.Text,
            CreatedDate = model.CreatedDate,
            TodoTaskId = model.TodoTaskId,
        };
    }

    public Task AddCommentAsync(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        return this.AddCommentAsyncCore(comment);
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await this.httpclient.DeleteAsync(new Uri($"Comments/{id}", UriKind.Relative));
        response.EnsureSuccessStatusCode();
    }

    public Task UpdateCommentAsync(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        return this.UpdateCommentAsyncCore(comment);
    }

    private async Task AddCommentAsyncCore(Comment comment)
    {
        var model = new CommentWebApiModel
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedDate = comment.CreatedDate,
            TodoTaskId = comment.TodoTaskId,
        };

        var result = await this.httpclient.PostAsJsonAsync("Comments", model);
        result.EnsureSuccessStatusCode();
    }

    private async Task UpdateCommentAsyncCore(Comment comment)
    {
        var model = new CommentWebApiModel
        {
            Id = comment.Id,
            Text = comment.Text,
            TodoTaskId = comment.TodoTaskId,
        };

        var result = await this.httpclient.PutAsJsonAsync($"Comments/{comment.Id}", model);
        result.EnsureSuccessStatusCode();
    }
}
