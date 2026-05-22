namespace TodoListApp.Data.Entities;

/// <summary>
/// Entity representing a to-do list in the database.
/// </summary>
public class TodoListEntity
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

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns this list.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    public ICollection<TodoTaskEntity> Tasks { get;  } = new List<TodoTaskEntity>();
}
