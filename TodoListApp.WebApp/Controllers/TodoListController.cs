using System;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class TodoListController : Controller
{
    private readonly ITodoListWebApiService service;

    public TodoListController(ITodoListWebApiService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var lists = await this.service.GetTodoListsAsync();
        return this.View(lists);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return this.View(new TodoListWebApiModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<ActionResult> Create(TodoListWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.CreateCore(model);
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        var list = await this.service.GetTodoListByIdAsync(id);
        if (list == null)
        {
            return this.NotFound();
        }

        var model = new TodoListWebApiModel
        {
            Id = list.Id,
            Title = list.Title,
            Description = list.Description,
        };
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, TodoListWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.EditCore(id, model);
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
        var list = await this.service.GetTodoListByIdAsync(id);
        if (list == null)
        {
            return this.NotFound();
        }

        return this.View(list);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        await this.service.DeleteTodoListAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }

    private async Task<ActionResult> CreateCore(TodoListWebApiModel model)
    {
        if (this.ModelState.IsValid)
        {
            var todoList = new TodoList()
            {
                Title = model.Title,
                Description = model.Description,
            };
            await this.service.AddTodoListAsync(todoList);
            return this.RedirectToAction(nameof(this.Index));
        }

        return this.View(model);
    }

    private async Task<IActionResult> EditCore(int id, TodoListWebApiModel model)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (this.ModelState.IsValid)
        {
            var todoList = new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
            };
            await this.service.UpdateTodoListAsync(todoList);
            return this.RedirectToAction(nameof(this.Index));
        }

        return this.View(model);
    }
}
