# NXTApp

NXTApp is an open-source peer-to-peer education platform designed as a monorepo to manage various components of the application. The repository contains multiple services, primarily written in C#, alongside modern frontend technologies like Svelte and TypeScript.

<img width="1311" height="1406" alt="image" src="https://github.com/user-attachments/assets/0eea5b7d-9705-43dc-a8a4-f44c5f3e589d" />


## Repository Structure

- **Backend**: Contains the backend services for the application, written in C#.
  - `NXTBackend.API`: Main API service.
  - `NXTBackend.API.Core`: Core business logic and reusable utilities.
  - `NXTBackend.API.Models`: Data models used across services.
  - `NXTBackend.API.Infrastructure`: Database and external service interactions.
  - `NXTBackend.API.Domain`: Domain-specific definitions.
  - `NXTBackend.API.Tests`: Unit tests for backend services.

- **Frontend**: Built using Svelte and TypeScript.

- **Docker**: Configuration for containerizing the application.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Keycloak](https://www.keycloak.org/)
- [Docker](https://www.docker.com/)

## Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/W2Inc/NXTApp.git
   cd NXTApp
   ```

2. Set up required services:
   - Install and configure PostgreSQL.
   - Set up Keycloak for authentication and authorization.
   - Set up a Gitea (or Forgejo) as the backend relies on it's API

3. Configure environment variables:
   - Refer to environment variable settings in individual project READMEs.

4. Run the application:
   ```bash
   dotnet build
   dotnet run --project backend/NXTBackend.API/NXTBackend.API.csproj
   ```

5. Access the application at `http://localhost:5000`.

## Git Server

There are various solutions to setting up a git server but in our case we simply use

## Contributing

Contributions are welcome! Please see our [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
