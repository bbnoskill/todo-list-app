using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.Data.Entities;

/// <summary>
/// Entity representing a comment for a to-do task.
/// </summary>
public class CommentEntity
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date of the comment.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the associated task.
    /// </summary>
    public int TodoTaskId { get; set; }

    /// <summary>
    /// Gets or sets the associated to-do task.
    /// </summary>
    [ForeignKey(nameof(TodoTaskId))]
    public TodoTaskEntity Task { get; set; } = null!;
}
