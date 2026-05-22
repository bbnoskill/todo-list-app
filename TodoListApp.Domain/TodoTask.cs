namespace TodoListApp.Domain;

/// <summary>
/// Domain model representing a to-do list.
/// </summary>
public class TodoTask
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
}
