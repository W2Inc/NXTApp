# NXTBackend.API

This is the main API project for the NXTApp platform.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Keycloak](https://www.keycloak.org/)

## Setup Instructions

1. **Install Dependencies**:
   ```bash
   dotnet restore
   ```

2. **Configure Environment Variables**:
   - Add a `UserSecretsId` in `appsettings.json` to manage sensitive configurations.
   - Example:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Host=localhost;Database=NXTApp;Username=postgres;Password=password"
       },
       "Keycloak": {
         "Realm": "nxtapp",
         "ClientId": "nxtapp-client",
         "ClientSecret": "secret"
       }
     }
     ```

3. **Run Migrations**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the Application**:
   ```bash
   dotnet run
   ```

5. **Access the API**:
   The API will be available at `http://localhost:5000`.

## Key Features

- Authentication via Keycloak.
- Entity Framework Core for database management.
- Redis caching.

## Dependencies

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.EntityFrameworkCore.Design`
- `Quartz.AspNetCore`
- `Serilog.AspNetCore`