using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly ITodoTaskWebApiService service;

    public SearchController(ITodoTaskWebApiService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? searchQuery, [FromQuery] string? searchBy)
    {
        this.ViewBag.SearchQuery = searchQuery;
        this.ViewBag.SearchBy = searchBy;

        if (string.IsNullOrWhiteSpace(searchQuery) || string.IsNullOrWhiteSpace(searchBy))
        {
            return this.View(Enumerable.Empty<Models.TodoTask>());
        }

        var tasks = await this.service.SearchTodoTasksAsync(searchQuery, searchBy);
        return this.View(tasks);
    }
}

