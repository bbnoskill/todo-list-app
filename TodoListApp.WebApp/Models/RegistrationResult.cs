using System.Collections.ObjectModel;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents the result of a registration attempt.
/// </summary>
public class RegistrationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the registration was successful.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets the list of error messages if registration failed.
    /// </summary>
    public Collection<string> Errors { get; } = new ();
}

/// <summary>
/// Helper class to deserialize Identity errors from Web API.
/// </summary>
public class WebApiIdentityError
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
