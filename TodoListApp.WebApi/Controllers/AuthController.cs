using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Domain;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenService tokenService;

    public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    [HttpPost("register")]
    public Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.RegisterCore(model);
    }

    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] LoginModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.LoginCore(model);
    }

    private async Task<IActionResult> RegisterCore(RegisterModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var user = new IdentityUser { UserName = model.Name, Email = model.Email, };
        var result = await this.userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var modelError in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, modelError.Description);
            }

            return this.BadRequest(result.Errors);
        }

        return this.Ok(new { Message = "User registered successfully!" });
    }

    private async Task<IActionResult> LoginCore(LoginModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var user = await this.userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return this.Unauthorized(new { Message = "Invalid username or password!" });
        }

        var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
        {
            return this.Unauthorized(new { Message = "Invalid username or password!" });
        }

        var token = this.tokenService.CreateToken(user);
        return this.Ok(new { Token = token });
    }
}
