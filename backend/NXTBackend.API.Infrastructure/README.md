# NXTBackend.API.Infrastructure

This project handles database and external service interactions for NXTApp.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/)

## Setup Instructions

1. **Install Dependencies**:
   ```bash
   dotnet restore
   ```

2. **Configure Environment Variables**:
   - Add database connection strings to `appsettings.json`.

3. **Run Migrations**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

## Key Features

- Database management with Entity Framework Core.
- PostgreSQL as the primary database.
- Migration support.

## Dependencies

- `Microsoft.EntityFrameworkCore`
- `Npgsql.EntityFrameworkCore.PostgreSQL`