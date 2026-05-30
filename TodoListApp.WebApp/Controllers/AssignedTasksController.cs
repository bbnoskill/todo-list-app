using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class AssignedTasksController : Controller
{
    private readonly ITodoTaskWebApiService service;

    public AssignedTasksController(ITodoTaskWebApiService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? isCompleted = null, [FromQuery] string sortBy = "duedate")
    {
        bool? filterValue = null;
        if (bool.TryParse(isCompleted, out bool parsed))
        {
            filterValue = parsed;
        }

        var tasks = await this.service.GetAssignedTodoTasksAsync(filterValue, sortBy);

        this.ViewBag.CurrentFilter = filterValue;
        this.ViewBag.CurrentSort = sortBy;

        return this.View(tasks);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, bool isCompleted, bool? currentFilter, string currentSort)
    {
        await this.service.ChangeTaskStatusAsync(id, isCompleted);
        return this.RedirectToAction(nameof(this.Index), new { isCompleted = currentFilter, sortBy = currentSort });
    }
}

