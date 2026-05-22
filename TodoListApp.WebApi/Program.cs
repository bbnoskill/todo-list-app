using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoListDbConnection")
        ?? throw new InvalidOperationException("Connection string 'TodoListDbConnection' not found.")));

// --- Identity ---
builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<TodoListDbContext>()
.AddDefaultTokenProviders();

// --- JWT Authentication ---
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    };
});

// --- Your existing services ---
builder.Services.AddScoped<TodoListApp.Data.Interfaces.ITodoListRepository, TodoListApp.Data.Repositories.TodoListRepository>();
builder.Services.AddScoped<TodoListApp.Data.Interfaces.ITodoTaskRepository, TodoListApp.Data.Repositories.TodoTaskRepository>();
builder.Services.AddScoped<TodoListApp.Services.ITodoListDatabaseService, TodoListApp.Services.TodoListDatabaseService>();
builder.Services.AddScoped<TodoListApp.Services.ITodoTaskDatabaseService, TodoListApp.Services.TodoTaskDatabaseService>();
builder.Services.AddScoped<TodoListApp.Services.ITagDatabaseService, TodoListApp.Services.TagDatabaseService>();
builder.Services.AddScoped<TodoListApp.Services.ICommentDatabaseService, TodoListApp.Services.CommentDatabaseService>();
builder.Services.AddScoped<TodoListApp.Services.ITokenService, TodoListApp.Services.TokenService>();
builder.Services.AddScoped<TodoListApp.Data.Interfaces.ITaskHistoryRepository, TodoListApp.Data.Repositories.TaskHistoryRepository>();
builder.Services.AddScoped<TodoListApp.Services.ITaskHistoryDatabaseService, TodoListApp.Services.TaskHistoryDatabaseService>();

var app = builder.Build();

// Automatically apply migrations and create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoListDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
