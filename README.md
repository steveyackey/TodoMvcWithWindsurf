# MvcApp

A modern ASP.NET Core MVC Todo application with clean architecture, MediatR CQRS, and SQLite persistence. Includes unit and UI acceptance tests.

## Features

- Add, complete, and delete todos
- Set due date, priority, and category
- Tagging and attachments support
- Responsive Bootstrap UI
- SQLite database with EF Core migrations
- CQRS with MediatR
- Unit and UI acceptance tests

## Architecture Patterns

- **Model-View-Controller (MVC):** Organizes code into controllers, views, and models for separation of concerns.
- **CQRS (Command Query Responsibility Segregation):** Uses MediatR to separate read (query) and write (command) logic.
- **Clean Architecture:** Layers for domain, application/features, infrastructure/data, and UI, promoting testability and maintainability.
- **Entity Framework Core:** For database access and migrations (SQLite provider).

## Project Structure

- `MvcApp/` - Main web application (controllers, domain, data, features, views)
- `MvcApp.Tests/` - Unit tests for domain and features
- `MvcApp.UiTests/` - UI acceptance tests

## Getting Started

### Prerequisites

- .NET 8 SDK or later

### Setup

1. Clone the repository
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Run database migrations (auto-creates on first run):
   ```sh
   dotnet run --project MvcApp/MvcApp.csproj
   ```
4. Visit https://localhost:5001

### Running Tests

- Unit tests:
  ```sh
  dotnet test MvcApp.Tests
  ```
- UI acceptance tests:
  ```sh
  dotnet test MvcApp.UiTests
  ```

## Configuration

- Edit `MvcApp/appsettings.json` for connection strings and logging.
- Default DB: `todo.db` (SQLite, auto-created).

## License

MIT (add your license file if needed)
