using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services;

public interface IAuthWebService
{
    Task<string?> LoginAsync(LoginWebApiModel model);

    /// <summary>
    /// Registers a new user via Web API.
    /// </summary>
    /// <param name="model">The registration view model.</param>
    /// <returns>A RegistrationResult indicating success or failure with error messages.</returns>
    Task<RegistrationResult> RegisterAsync(RegisterWebApiModel model);
}
