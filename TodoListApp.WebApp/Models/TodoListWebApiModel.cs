using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class TodoListWebApiModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
