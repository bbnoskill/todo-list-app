using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing task history.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly ITaskHistoryDatabaseService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryController"/> class.
    /// </summary>
    /// <param name="service">The task history database service.</param>
    public HistoryController(ITaskHistoryDatabaseService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets the last 10 completed and deleted task history records.
    /// </summary>
    /// <returns>A collection of task history records.</returns>
    [HttpGet]
    public Task<ActionResult<IEnumerable<TaskHistoryModel>>> GetHistory()
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ArgumentNullException.ThrowIfNull(userId);
        return this.GetHistoryCore(userId);
    }

    private async Task<ActionResult<IEnumerable<TaskHistoryModel>>> GetHistoryCore(string userId)
    {
        var history = await this.service.GetRecentHistoryAsync(userId);
        return this.Ok(history.Select(h => new TaskHistoryModel
        {
            Id = h.Id,
            TaskTitle = h.TaskTitle,
            Action = h.Action,
            ActionDate = h.ActionDate,
        }));
    }
}
