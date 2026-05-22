using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing to-do tasks.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITodoTaskDatabaseService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksController"/> class.
    /// </summary>
    /// <param name="service">The database service.</param>
    public TasksController(ITodoTaskDatabaseService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets a to-do task by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The to-do task.</returns>
    [HttpGet("{id}")]
    public Task<ActionResult<TodoTaskModel>> GetById(int id)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetByIdCore(id, userId);
    }

    [HttpGet("assigned")]
    public Task<ActionResult<IEnumerable<TodoTaskModel>>> GetAssigned([FromQuery] bool? isCompleted = null, [FromQuery] string? sortBy = "duedate")
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetAssignedCore(userId, isCompleted, sortBy);
    }

    [HttpGet("search")]
    public Task<ActionResult<IEnumerable<TodoTaskModel>>> Search([FromQuery] string? searchQuery, [FromQuery] string? searchBy)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.SearchCore(userId, searchQuery, searchBy);
    }

    /// <summary>
    /// Updates an existing to-do task.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The to-do task model.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.UpdateCore(id, model, userId);
    }

    [HttpPatch("{id}/status")]
    public Task<IActionResult> UpdateStatus(int id, [FromBody] bool isCompleted)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.UpdateStatusCore(id, userId, isCompleted);
    }

    /// <summary>
    /// Deletes a to-do task.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(int id)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.DeleteCore(id, userId);
    }

    private async Task<ActionResult<TodoTaskModel>> GetByIdCore(int id, string userId)
    {
        var task = await this.service.GetTodoTaskByIdAsync(id, userId);
        if (task == null)
        {
            return this.NotFound();
        }

        return this.Ok(new TodoTaskModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            TodoListId = task.TodoListId,
        });
    }

    private async Task<ActionResult<IEnumerable<TodoTaskModel>>> GetAssignedCore(string userId, bool? isCompleted, string? sortBy)
    {
        var tasks = await this.service.GetAssignedTodoTasksAsync(userId, isCompleted, sortBy);

        return this.Ok(tasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        }));
    }

    private async Task<ActionResult<IEnumerable<TodoTaskModel>>> SearchCore(string userId, string? searchQuery, string? searchBy)
    {
        var tasks = await this.service.SearchTodoTasksAsync(userId, searchQuery, searchBy);
        return this.Ok(tasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            CreatedDate = t.CreatedDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        }));
    }

    private async Task<IActionResult> UpdateCore(int id, TodoTaskModel model, string userId)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var existing = await this.service.GetTodoTaskByIdAsync(id, userId);
        if (existing == null)
        {
            return this.NotFound();
        }

        var domainModel = new TodoTask
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            IsCompleted = model.IsCompleted,
            TodoListId = model.TodoListId,
        };

        await this.service.UpdateTodoTaskAsync(domainModel);

        return this.NoContent();
    }

    private async Task<IActionResult> UpdateStatusCore(int id, string userId, bool isCompleted)
    {
        var existing = await this.service.GetTodoTaskByIdAsync(id, userId);
        if (existing == null)
        {
            return this.NotFound();
        }

        await this.service.ChangeTaskStatusAsync(id, isCompleted, userId);

        return this.NoContent();
    }

    private async Task<IActionResult> DeleteCore(int id, string userId)
    {
        var existing = await this.service.GetTodoTaskByIdAsync(id, userId);
        if (existing == null)
        {
            return this.NotFound();
        }

        await this.service.DeleteTodoTaskAsync(id, userId);

        return this.NoContent();
    }
}
