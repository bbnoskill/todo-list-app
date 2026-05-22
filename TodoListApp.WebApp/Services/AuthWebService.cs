using TodoListApp.WebApp.Models;
namespace TodoListApp.WebApp.Services;

public class AuthWebService : IAuthWebService
{
    private readonly HttpClient httpclient;

    public AuthWebService(HttpClient httpclient)
    {
        this.httpclient = httpclient;
    }

    public Task<string?> LoginAsync(LoginWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.LoginAsyncCore(model);
    }

    public Task<RegistrationResult> RegisterAsync(RegisterWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.RegisterAsyncCore(model);
    }

    private async Task<string?> LoginAsyncCore(LoginWebApiModel model)
    {
        var response = await this.httpclient.PostAsJsonAsync("Auth/Login", new { model.Email, model.Password });
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return result?.Token;
    }

    /// <summary>
    /// Registers a new user via Web API.
    /// </summary>
    /// <param name="model">The registration view model.</param>
    /// <returns>A RegistrationResult indicating success or failure with error messages.</returns>
    private async Task<RegistrationResult> RegisterAsyncCore(RegisterWebApiModel model)
    {
        var response = await this.httpclient.PostAsJsonAsync("Auth/Register", new { model.Name, model.Email, model.Password, model.ConfirmPassword });

        if (response.IsSuccessStatusCode)
        {
            return new RegistrationResult { Succeeded = true };
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errors = await response.Content.ReadFromJsonAsync<List<WebApiIdentityError>>();
            if (errors != null)
            {
                var result = new RegistrationResult { Succeeded = false };
                foreach (var error in errors)
                {
                    result.Errors.Add(error.Description);
                }

                return result;
            }
        }

        var unexpectedResult = new RegistrationResult { Succeeded = false };
        unexpectedResult.Errors.Add("An unexpected error occurred during registration.");
        return unexpectedResult;
    }
}
