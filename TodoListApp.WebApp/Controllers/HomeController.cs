using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// The default home controller.
/// </summary>
public class HomeController : Controller
{
    private readonly ITodoListWebApiService todoListService;
    private readonly ITodoTaskWebApiService todoTaskService;
    private readonly ITagWebApiService tagService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="todoListService">The to-do list service.</param>
    /// <param name="todoTaskService">The to-do task service.</param>
    /// <param name="tagService">The tag service.</param>
    public HomeController(
        ILogger<HomeController> logger,
        ITodoListWebApiService todoListService,
        ITodoTaskWebApiService todoTaskService,
        ITagWebApiService tagService)
    {
        _ = logger;
        this.todoListService = todoListService;
        this.todoTaskService = todoTaskService;
        this.tagService = tagService;
    }

    /// <summary>
    /// Handles the index action.
    /// </summary>
    /// <returns>The index view.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
        {
            try
            {
                var lists = await this.todoListService.GetTodoListsAsync();
                this.ViewBag.TopLists = lists.Take(3).ToList();

                var tasks = await this.todoTaskService.GetAssignedTodoTasksAsync(false, "duedate");
                this.ViewBag.TopTasks = tasks.Take(3).ToList();

                var tags = await this.tagService.GetAllTagsAsync();
                var tagList = tags.ToList();

                // Count tasks per tag to sort by popularity
                var tagWithCounts = new List<(Tag Tag, int Count)>();
                foreach (var tag in tagList)
                {
                    var tagTasks = await this.tagService.GetTasksByTagAsync(tag.Id);
                    tagWithCounts.Add((tag, tagTasks.Count()));
                }

                this.ViewBag.TopTags = tagWithCounts
                    .OrderByDescending(t => t.Count)
                    .Take(3)
                    .Select(t => new { t.Tag, t.Count })
                    .ToList();
            }
            catch (HttpRequestException)
            {
                this.ViewBag.TopLists = new List<TodoList>();
                this.ViewBag.TopTasks = new List<TodoTask>();
                this.ViewBag.TopTags = new List<object>();
            }
        }

        return this.View();
    }

    /// <summary>
    /// Handles the privacy action.
    /// </summary>
    /// <returns>The privacy view.</returns>
    [HttpGet]
    public IActionResult Privacy()
    {
        return this.View();
    }

    /// <summary>
    /// Handles errors.
    /// </summary>
    /// <returns>The error view.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
