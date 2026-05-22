using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class AccountController : Controller
{
    private readonly IAuthWebService authService;

    public AccountController(IAuthWebService authService)
    {
        this.authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Login(LoginWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.LoginCore(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Register(RegisterWebApiModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return this.RegisterCore(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return this.RedirectToAction("Index", "Home");
    }

    private async Task<IActionResult> LoginCore(LoginWebApiModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var token = await this.authService.LoginAsync(model);
        if (string.IsNullOrEmpty(token))
        {
            this.ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return this.View(model);
        }

        await this.SignInUserAsync(token);
        return this.RedirectToAction("Index", "Home");
    }

    private async Task<IActionResult> RegisterCore(RegisterWebApiModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var result = await this.authService.RegisterAsync(model);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }

            return this.View(model);
        }

        // Автоматичний вхід після реєстрації
        var loginModel = new LoginWebApiModel { Email = model.Email, Password = model.Password };
        var token = await this.authService.LoginAsync(loginModel);
        if (!string.IsNullOrEmpty(token))
        {
            await this.SignInUserAsync(token);
            return this.RedirectToAction("Index", "Home");
        }

        return this.RedirectToAction("Login");
    }

    private async Task SignInUserAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claims = new List<Claim>();

        // Map JWT claims to standard ClaimTypes
        foreach (var claim in jwtToken.Claims)
        {
            if (claim.Type == JwtRegisteredClaimNames.NameId || claim.Type == "nameid")
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
            }
            else if (claim.Type == JwtRegisteredClaimNames.UniqueName || claim.Type == "unique_name")
            {
                claims.Add(new Claim(ClaimTypes.Name, claim.Value));
            }
            else if (claim.Type == JwtRegisteredClaimNames.Email || claim.Type == "email")
            {
                claims.Add(new Claim(ClaimTypes.Email, claim.Value));
            }
            else
            {
                claims.Add(claim);
            }
        }

        claims.Add(new Claim("jwt_token", token));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await this.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true });
    }
}
