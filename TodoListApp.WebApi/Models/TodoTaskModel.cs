using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

/// <summary>
/// Model for a to-do task in API requests and replies.
/// </summary>
public class TodoTaskModel
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the due date.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is completed.
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the linked to-do list identifier.
    /// </summary>
    public int TodoListId { get; set; }
}
