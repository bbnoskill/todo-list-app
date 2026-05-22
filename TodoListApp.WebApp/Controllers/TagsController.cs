using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class TagsController : Controller
{
    private readonly ITagWebApiService service;

    public TagsController(ITagWebApiService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tags = await this.service.GetAllTagsAsync();
        return this.View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var tasks = await this.service.GetTasksByTagAsync(id);
        this.ViewBag.TagId = id;
        return this.View(tasks);
    }
}
