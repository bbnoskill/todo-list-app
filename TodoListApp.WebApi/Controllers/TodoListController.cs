using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing to-do lists.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListController"/> class.
    /// </summary>
    /// <param name="service">The database service.</param>
    public TodoListController(ITodoListDatabaseService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets all to-do lists.
    /// </summary>
    /// <returns>A collection of to-do lists.</returns>
    [HttpGet]
    public Task<ActionResult<IEnumerable<TodoListModel>>> GetAll()
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetAllCore(userId);
    }

    /// <summary>
    /// Gets a to-do list by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The to-do list.</returns>
    [HttpGet("{id}")]
    public Task<ActionResult<TodoListModel>> GetById(int id)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetByIdCore(id, userId);
    }

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="model">The to-do list model.</param>
    /// <returns>The created to-do list.</returns>
    [HttpPost]
    public Task<ActionResult<TodoListModel>> Create(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.CreateCore(model, userId);
    }

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The to-do list model.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.UpdateCore(id, model, userId);
    }

    /// <summary>
    /// Deletes a to-do list.
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

    /// <summary>
    /// Gets all tasks for a specific to-do list.
    /// </summary>
    /// <param name="id">The identifier of the to-do list.</param>
    /// <param name="taskService">The task database service.</param>
    /// <returns>A collection of tasks.</returns>
    [HttpGet("{id}/tasks")]
    public Task<ActionResult<IEnumerable<TodoTaskModel>>> GetTasks(int id, [FromServices] ITodoTaskDatabaseService taskService)
    {
        ArgumentNullException.ThrowIfNull(taskService);
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetTasksCore(id, userId, taskService);
    }

    /// <summary>
    /// Creates a new task in a specific to-do list.
    /// </summary>
    /// <param name="id">The identifier of the to-do list.</param>
    /// <param name="model">The task model.</param>
    /// <param name="taskService">The task database service.</param>
    /// <returns>The created task.</returns>
    [HttpPost("{id}/tasks")]
    public Task<ActionResult<TodoTaskModel>> CreateTask(int id, TodoTaskModel model, [FromServices] ITodoTaskDatabaseService taskService)
    {
        ArgumentNullException.ThrowIfNull(taskService);
        ArgumentNullException.ThrowIfNull(model);
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.CreateTaskCore(id, model, userId, taskService);
    }

    private async Task<ActionResult<IEnumerable<TodoListModel>>> GetAllCore(string userId)
    {
        var lists = await this.service.GetTodoListsAsync(userId);
        return this.Ok(lists.Select(l => new TodoListModel
        {
            Id = l.Id,
            Title = l.Title,
            Description = l.Description,
        }));
    }

    private async Task<ActionResult<TodoListModel>> GetByIdCore(int id, string userId)
    {
        var list = await this.service.GetTodoListByIdAsync(id, userId);
        if (list == null)
        {
            return this.NotFound();
        }

        return this.Ok(new TodoListModel
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
        });
    }

    private async Task<ActionResult<TodoListModel>> CreateCore(TodoListModel model, string userId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var domainModel = new TodoList
        {
            Title = model.Title,
            Description = model.Description,
        };

        await this.service.AddTodoListAsync(domainModel, userId);

        model.Id = domainModel.Id;
        return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
    }

    private async Task<IActionResult> UpdateCore(int id, TodoListModel model, string userId)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var existing = await this.service.GetTodoListByIdAsync(id, userId);
        if (existing == null)
        {
            return this.NotFound();
        }

        var domainModel = new TodoList
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
        };

        await this.service.UpdateTodoListAsync(domainModel);

        return this.NoContent();
    }

    private async Task<IActionResult> DeleteCore(int id, string userId)
    {
        var existing = await this.service.GetTodoListByIdAsync(id, userId);
        if (existing == null)
        {
            return this.NotFound();
        }

        await this.service.DeleteTodoListAsync(id);

        return this.NoContent();
    }

    private async Task<ActionResult<IEnumerable<TodoTaskModel>>> GetTasksCore(int id, string userId, ITodoTaskDatabaseService taskService)
    {
        var list = await this.service.GetTodoListByIdAsync(id, userId);
        if (list == null)
        {
            return this.NotFound();
        }

        var tasks = await taskService.GetTodoTasksAsync(userId);
        var listTasks = tasks.Where(t => t.TodoListId == id);
        return this.Ok(listTasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        }));
    }

    private async Task<ActionResult<TodoTaskModel>> CreateTaskCore(int id, TodoTaskModel model, string userId, ITodoTaskDatabaseService taskService)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var list = await this.service.GetTodoListByIdAsync(id, userId);
        if (list == null)
        {
            return this.NotFound();
        }

        var domainTask = new TodoTask
        {
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            IsCompleted = model.IsCompleted,
            TodoListId = id,
        };

        await taskService.AddTodoTaskAsync(domainTask);

        model.Id = domainTask.Id;
        model.TodoListId = id;

        return this.CreatedAtAction("GetById", "Tasks", new { id = model.Id }, model);
    }
}

