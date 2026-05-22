using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface IHistoryWebApiService
{
    Task<IEnumerable<TaskHistoryWebApiModel>> GetRecentHistoryAsync();
}
