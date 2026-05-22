namespace TodoListApp.Domain;

/// <summary>
/// Domain model for a comment.
/// </summary>
public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public int TodoTaskId { get; set; }
}
