# Changelog

All notable changes to this project will be documented in this file.

## [2026-03-07] - Initial Setup & Code Analysis
- Created and configured ASP.NET Core Web API (`TodoListApp.WebApi`) and MVC (`TodoListApp.WebApp`) applications.
- Created `Domain`, `Data`, `Services`, and `Shared` class libraries.
- Added all projects to `TodoListApp.sln`.
- Established dependencies between the projects following the 3-tier architecture.
- Added `Directory.Build.props` to enable .NET Analyzers and `StyleCop.Analyzers` globally for all projects.
- Fixed default StyleCop warnings (SA1101, SA1309, SA1516) and suppressed acceptable ones (CA1515, CA1716, NETSDK1138) for a clean build.

## [2026-03-07] - Database Context & Configuration
- Configured Entity Framework Core (`6.0.33`) packages for SQL Server in `WebApi` and `Data` projects.
- Created `TodoListDbContext` empty class in `TodoListApp.WebApi`.
- Added `TodoListDbConnection` localdb connection string to `appsettings.json`.
- Registered `TodoListDbContext` in `TodoListApp.WebApi` DI container using `UseSqlServer`.

## [2026-03-07] - Epic 1 Backend Implementation (To-Do Lists)
- Added `TodoListEntity` in `TodoListApp.Data` and added `DbSet` to `TodoListDbContext`.
- Implemented `ITodoListRepository` and `TodoListRepository` in `TodoListApp.Data`.
- Created `TodoList` domain model in `TodoListApp.Domain`.
- Implemented `ITodoListDatabaseService` and `TodoListDatabaseService` in `TodoListApp.Services`.
- Added `TodoListModel` and RESTful `TodoListController` in `TodoListApp.WebApi`.
- Registered mapping services in `Program.cs`.

