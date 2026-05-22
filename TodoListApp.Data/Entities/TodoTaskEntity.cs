namespace TodoListApp.Data.Entities;

/// <summary>
/// Entity representing a to-do list in the database.
/// </summary>
public class TodoTaskEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the to-do list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the to-do list.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    public bool IsCompleted { get; set; }

    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;

    public ICollection<TagEntity> Tags { get; } = new List<TagEntity>();

    /// <summary>
    /// Gets or sets the comments associated with the task.
    /// </summary>
    public ICollection<CommentEntity> Comments { get; } = new List<CommentEntity>();
}
