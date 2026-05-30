using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class CommentsController : Controller
{
    private readonly ICommentWebApiService commentService;

    public CommentsController(ICommentWebApiService commentService)
    {
        this.commentService = commentService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int taskId, string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var comment = new Comment { TodoTaskId = taskId, Text = text, };
            await this.commentService.AddCommentAsync(comment);
        }

        return this.RedirectToAction("Edit", "TodoTask", new { id = taskId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await this.commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return this.NotFound();
        }

        return this.View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(Comment model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.EditCore(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int taskId)
    {
        await this.commentService.DeleteCommentAsync(id);
        return this.RedirectToAction("Edit", "TodoTask", new { id = taskId });
    }

    private async Task<IActionResult> EditCore(Comment model)
    {
        if (this.ModelState.IsValid)
        {
            await this.commentService.UpdateCommentAsync(model);
            return this.RedirectToAction("Edit", "TodoTask", new { id = model.TodoTaskId });
        }

        return this.View(model);
    }
}

