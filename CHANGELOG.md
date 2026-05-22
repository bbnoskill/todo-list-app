# Changelog

All notable changes to this project will be documented in this file.

## [2026-05-22] - Dashboard UI Refactoring & Final Polish
- Refactored the Home page dashboard to use styled, clickable cards instead of plain text.
- Added redirect routing from dashboard items directly to list and task detail pages.
- Configured automatic Database Migration on application startup (`context.Database.Migrate()`).
- Added comprehensive `README.md` guide for reviewers.

## [2026-05-20] - Task History & Tags Implementation
- Implemented `TaskHistoryEntity` and tracking services to log the last 10 completed/deleted tasks per user.
- Created `TagsController` and `TagWebApiService` for the WebApp.
- Added UI for tagging tasks and filtering tasks by tags.
- Fixed JWT token expiration behavior and HTTP-to-HTTPS redirect drops for `HttpClient`.

## [2026-05-18] - User Authentication & Identity
- Integrated ASP.NET Core Identity for user management.
- Set up JWT Bearer Token generation in WebApi (`TokenService`).
- Created `AccountController` in WebApp for Login/Registration flow.
- Added `JwtTokenHandler` delegating handler to automatically attach tokens to API requests.

## [2026-05-15] - Epic 2 Backend & Frontend (Tasks Management)
- Added `TodoTaskEntity` with completion status and due date fields.
- Implemented `TodoTaskController` for RESTful operations.
- Created Razor views (`Create`, `Edit`, `Index`, `Details`) for Task management in the WebApp.
- Configured one-to-many relationship between Lists and Tasks.

## [2026-05-09] - Epic 1 Backend Implementation (To-Do Lists)
- Added `TodoListEntity` in `TodoListApp.Data` and added `DbSet` to `TodoListDbContext`.
- Implemented `ITodoListRepository` and `TodoListRepository` in `TodoListApp.Data`.
- Created `TodoList` domain model in `TodoListApp.Domain`.
- Implemented `ITodoListDatabaseService` and `TodoListDatabaseService` in `TodoListApp.Services`.
- Added `TodoListModel` and RESTful `TodoListController` in `TodoListApp.WebApi`.

## [2026-05-06] - Database Context & Configuration
- Configured Entity Framework Core packages for SQL Server.
- Created `TodoListDbContext` in `TodoListApp.WebApi`.
- Added `TodoListDbConnection` localdb connection string to `appsettings.json`.
- Registered `TodoListDbContext` in DI container using `UseSqlServer`.

## [2026-05-04] - Initial Setup & Code Analysis
- Created and configured ASP.NET Core Web API (`TodoListApp.WebApi`) and MVC (`TodoListApp.WebApp`) applications.
- Created `Domain`, `Data`, `Services`, and `Shared` class libraries.
- Established dependencies between the projects following the 3-tier architecture.
- Added `Directory.Build.props` to enable .NET Analyzers and `StyleCop.Analyzers` globally.
- Fixed default StyleCop warnings for a clean build.
