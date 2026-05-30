using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class HistoryController : Controller
{
    private readonly IHistoryWebApiService service;

    public HistoryController(IHistoryWebApiService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var history = await this.service.GetRecentHistoryAsync();
        return this.View(history);
    }
}

