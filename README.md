# IlmPath API

A Clean Architecture ASP.NET Core Web API project.

## Project Structure

The solution follows Clean Architecture principles with the following layers:

- **IlmPath.Domain**: Contains enterprise logic and entities
- **IlmPath.Application**: Contains business logic and interfaces
- **IlmPath.Infrastructure**: Contains external concerns and implementations
- **IlmPath.Api**: Contains API endpoints and configuration

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or higher)

## Getting Started

1. Clone the repository
2. Navigate to the solution directory
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project IlmPath.Api
   ```

## Database Setup

The project uses Entity Framework Core with SQL Server. To set up the database:

1. Update the connection string in `IlmPath.Api/appsettings.json` if needed
2. Run the following commands:
   ```bash
   dotnet ef migrations add InitialCreate --project IlmPath.Infrastructure --startup-project IlmPath.Api
   dotnet ef database update --project IlmPath.Infrastructure --startup-project IlmPath.Api
   ```

## Architecture Overview

This project follows Clean Architecture principles:

1. **Domain Layer**: Core entities and business rules
2. **Application Layer**: Use cases and interfaces
3. **Infrastructure Layer**: External concerns (database, file system, etc.)
4. **API Layer**: Controllers and DTOs

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core 9.0
- SQL Server
- Clean Architecture 