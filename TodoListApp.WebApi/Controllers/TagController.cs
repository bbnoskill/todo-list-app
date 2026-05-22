using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagDatabaseService service;

    public TagController(ITagDatabaseService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<TagModel>> GetAllTags()
    {
        var tags = await this.service.GetAllTagsAsync();
        return this.Ok(tags.Select(t => new TagModel { Id = t.Id, Name = t.Name }));
    }

    [HttpGet("{taskId}/tags")]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetTagsForTask(int taskId)
    {
        var tags = await this.service.GetTagsForTaskAsync(taskId);
        return this.Ok(tags.Select(t => new TagModel { Id = t.Id, Name = t.Name }));
    }

    [HttpGet("{tagId}/tasks")]
    public async Task<ActionResult<IEnumerable<TodoTaskModel>>> GetTasksByTag(int tagId)
    {
        var tasks = await this.service.GetTasksByTagAsync(tagId);
        return this.Ok(tasks.Select(t => new TodoTaskModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            CreatedDate = t.CreatedDate,
            IsCompleted = t.IsCompleted,
            TodoListId = t.TodoListId,
        }));
    }

    [HttpPost("{taskId}/tags")]
    public async Task<IActionResult> AddTagToTask(int taskId, [FromBody] string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            return this.BadRequest();
        }

        await this.service.AddTagToTaskAsync(taskId, tagName);
        return this.NoContent();
    }

    [HttpDelete("{taskId}/tags/{tagId}")]
    public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
    {
        await this.service.RemoveTagFromTaskAsync(taskId, tagId);
        return this.NoContent();
    }
}
