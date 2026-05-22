using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public class HistoryWebApiService : IHistoryWebApiService
{
    private readonly HttpClient httpClient;

    public HistoryWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TaskHistoryWebApiModel>> GetRecentHistoryAsync()
    {
        var models = await this.httpClient.GetFromJsonAsync<IEnumerable<TaskHistoryWebApiModel>>("History");
        return models ?? Enumerable.Empty<TaskHistoryWebApiModel>();
    }
}
