using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService service;
    private readonly ITodoListWebApiService listService;
    private readonly ITagWebApiService tagService;
    private readonly ICommentWebApiService commentService;

    public TodoTaskController(ITodoTaskWebApiService service, ITodoListWebApiService listService, ITagWebApiService tagService, ICommentWebApiService commentService)
    {
        this.service = service;
        this.listService = listService;
        this.tagService = tagService;
        this.commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<TodoTask>> Index(int listId)
    {
        var list = await this.listService.GetTodoListByIdAsync(listId);
        if (list == null)
        {
            return this.NotFound();
        }

        this.ViewBag.TodoListTitle = list.Title;
        this.ViewBag.TodoListId = listId;

        var tasks = await this.service.GetTodoTasksAsync(listId);
        var activeTasks = tasks.Where(t => !t.IsCompleted);
        return this.View(activeTasks);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var task = await this.service.GetTodoTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        this.ViewBag.Tags = await this.tagService.GetTagsForTaskAsync(id);
        this.ViewBag.Comments = await this.commentService.GetCommentsForTaskAsync(id);
        return this.View(task);
    }

    [HttpGet]
    public IActionResult Create(int listId)
    {
        var model = new TodoTaskWebApiModel
        {
            TodoListId = listId,
            DueDate = DateTime.Now.AddDays(1),
        };
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(TodoTaskWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.CreateCore(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await this.service.GetTodoTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        var model = new TodoTaskWebApiModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            TodoListId = task.TodoListId,
        };

        this.ViewBag.Tags = await this.tagService.GetTagsForTaskAsync(id);
        this.ViewBag.Comments = await this.commentService.GetCommentsForTaskAsync(id);
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, TodoTaskWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.EditCore(id, model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await this.service.GetTodoTaskByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        return this.View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, int listId)
    {
        await this.service.DeleteTodoTaskAsync(id);
        return this.RedirectToAction(nameof(this.Index), new { listId = listId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTag(int id, string tagName)
    {
        if (!string.IsNullOrWhiteSpace(tagName))
        {
            await this.tagService.AddTagToTaskAsync(id, tagName);
        }

        return this.RedirectToAction(nameof(this.Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTag(int id, int tagId)
    {
        await this.tagService.RemoveTagFromTaskAsync(id, tagId);
        return this.RedirectToAction(nameof(this.Index), new { listId = id });
    }

    private async Task<IActionResult> CreateCore(TodoTaskWebApiModel model)
    {
        if (this.ModelState.IsValid)
        {
            var todoTask = new TodoTask
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                IsCompleted = model.IsCompleted,
                TodoListId = model.TodoListId,
            };
            await this.service.AddTodoTaskAsync(todoTask);
            return this.RedirectToAction(
                nameof(this.Index),
                new { listId = model.TodoListId });
        }

        return this.View(model);
    }

    private async Task<IActionResult> EditCore(int id, TodoTaskWebApiModel model)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (this.ModelState.IsValid)
        {
            var todoTask = new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                IsCompleted = model.IsCompleted,
                TodoListId = model.TodoListId,
            };
            await this.service.UpdateTodoTaskAsync(todoTask);
            return this.RedirectToAction(nameof(this.Index), new { listId = model.TodoListId });
        }

        return this.View(model);
    }
}

