# API

The `NXTBackend.API` project is the central hub for handling all API traffic. It includes the controllers, services, middleware, and other classes that are used to interact with the API. This project is responsible for defining all the various routes that the API will expose to the client and the logic that will be executed when those routes are hit.

## Key Components

### Controllers
Controllers define the endpoints that the API exposes. Each controller is responsible for handling specific routes and processing incoming HTTP requests. They interact with services to perform operations and return appropriate responses.

### Services
Services contain the business logic of the application. They are used by controllers to perform operations such as data retrieval, processing, and manipulation. Services interact with the database through repositories or directly using Entity Framework.

### Middleware
Middleware components are used to handle cross-cutting concerns such as authentication, logging, and error handling. They are executed in the order they are registered in the pipeline and can modify the request and response objects.

## Running Migrations

To add a new migration, use the following command:

```bash
dotnet ef migrations add <MigrationName> --project "NXTBackend.API.Infrastructure" -s "NXTBackend.API"
```

Replace `<MigrationName>` with the name of your migration. This command will create a new migration file in the `NXTBackend.API.Infrastructure` project.

To apply the migrations to the database, use the following command:

```bash
dotnet ef database update --project "NXTBackend.API.Infrastructure" -s "NXTBackend.API"
```

This command will apply all pending migrations to the database.
