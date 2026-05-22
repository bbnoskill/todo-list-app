using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing task comments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentDatabaseService service;

    public CommentsController(ICommentDatabaseService service)
    {
        this.service = service;
    }

    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<CommentModel>>> GetCommentsForTask(int taskId)
    {
        var comments = await this.service.GetCommentsForTaskAsync(taskId);
        return this.Ok(comments.Select(c => new CommentModel
        {
            Id = c.Id,
            Text = c.Text,
            CreatedDate = c.CreatedDate,
            TodoTaskId = c.TodoTaskId,
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentModel>> GetById(int id)
    {
        var comment = await this.service.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return this.NotFound();
        }

        return this.Ok(new CommentModel
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedDate = comment.CreatedDate,
            TodoTaskId = comment.TodoTaskId,
        });
    }

    [HttpPost]
    public Task<ActionResult<CommentModel>> Create(CommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.CreateCore(model);
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, CommentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.UpdateCore(id, model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await this.service.GetCommentByIdAsync(id);
        if (existing == null)
        {
            return this.NotFound();
        }

        await this.service.DeleteCommentAsync(id);

        return this.NoContent();
    }

    private async Task<ActionResult<CommentModel>> CreateCore(CommentModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var domainModel = new Comment
        {
            Text = model.Text,
            TodoTaskId = model.TodoTaskId,
        };

        await this.service.AddCommentAsync(domainModel);

        model.Id = domainModel.Id;
        model.CreatedDate = domainModel.CreatedDate;

        return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
    }

    private async Task<IActionResult> UpdateCore(int id, CommentModel model)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var existing = await this.service.GetCommentByIdAsync(id);
        if (existing == null)
        {
            return this.NotFound();
        }

        var domainModel = new Comment
        {
            Id = model.Id,
            Text = model.Text,
            CreatedDate = existing.CreatedDate,
            TodoTaskId = existing.TodoTaskId,
        };

        await this.service.UpdateCommentAsync(domainModel);

        return this.NoContent();
    }
}
