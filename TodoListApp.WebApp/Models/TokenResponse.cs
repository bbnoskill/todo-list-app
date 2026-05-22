namespace TodoListApp.WebApp.Models;

public record TokenResponse
{
    public string Token { get; init; } = string.Empty;
}
