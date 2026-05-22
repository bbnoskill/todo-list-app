# TodoList App - Capstone Project

## Overview
This is a comprehensive Todo List application consisting of a robust RESTful Web API backend and an intuitive ASP.NET Core MVC frontend. It was developed to track current tasks and manage daily productivity.

## Key Features
- **User Authentication:** Secure JWT-based registration and login system.
- **Todo Lists:** Create, edit, and delete multiple to-do lists to categorize tasks.
- **Task Management:** Add new tasks with due dates, descriptions, and mark them as completed. Completed tasks are automatically hidden from active views.
- **Tags:** Categorize tasks with tags for easy filtering and organization.
- **History Tracking:** The system automatically logs the history of the last 10 completed or deleted tasks per user.
- **Interactive Dashboard:** The home page provides a quick overview of upcoming tasks, prioritized lists, and popular tags.
- **Automatic Database Migration:** The application automatically provisions and migrates the SQL Server LocalDB database upon startup—no manual `Update-Database` required!

## Tech Stack
- **Backend:** ASP.NET Core Web API (.NET 6)
- **Frontend:** ASP.NET Core MVC WebApp (.NET 6), Bootstrap 5
- **Database:** Entity Framework Core, SQL Server LocalDB
- **Authentication:** ASP.NET Core Identity, JWT Bearer Tokens

## Getting Started (How to Run)

To evaluate this project, you need to launch both the Web API and the Web App simultaneously.

### 1. Launch the Web API
Open a terminal in the project root directory and run:
```bash
dotnet run --project TodoListApp.WebApi
```
> **Note:** The Web API will automatically create the `TodoListDbV2` database on your `(localdb)\mssqllocaldb` instance and apply all EF migrations. 

### 2. Launch the Web App
Open a second terminal window in the project root and run:
```bash
dotnet run --project TodoListApp.WebApp
```

### 3. Usage Guide
1. Open the Web App URL (usually `http://localhost:5272` or `https://localhost:7093`) in your browser.
2. Click **Register** to create a new user account.
3. Once logged in, navigate to **To-Do Lists** and click "Create New" to make a list.
4. Click on the list's title to view it, and add a **New Task**.
5. When a task is marked as completed (checkbox), it will disappear from the active list.
6. Navigate to **History** in the top navigation bar to see the automated audit log of your completed/deleted actions.
