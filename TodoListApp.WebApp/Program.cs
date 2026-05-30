using Microsoft.AspNetCore.Authentication.Cookies;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
}).AddHttpMessageHandler<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

builder.Services.AddHttpClient<ITodoTaskWebApiService, TodoTaskWebApiService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
}).AddHttpMessageHandler<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

builder.Services.AddHttpClient<ITagWebApiService, TagWebApiService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
}).AddHttpMessageHandler<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

builder.Services.AddHttpClient<ICommentWebApiService, CommentWebApiService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
}).AddHttpMessageHandler<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

builder.Services.AddHttpClient<IAuthWebService, AuthWebService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
});

builder.Services.AddHttpClient<IHistoryWebApiService, HistoryWebApiService>(client =>
{
    var baseAddress = builder.Configuration["WebApiBaseAddress"] ?? "https://localhost:7214/";
    client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
}).AddHttpMessageHandler<TodoListApp.WebApp.Handlers.JwtTokenHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

