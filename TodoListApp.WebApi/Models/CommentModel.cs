using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

/// <summary>
/// Model for a comment in API requests and responses.
/// </summary>
public class CommentModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public int TodoTaskId { get; set; }
}
