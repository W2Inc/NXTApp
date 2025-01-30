# NXT Application

A modern, self-hostable, and fully open-source education platform designed to change how we approach learning. NXT aims to break free from traditional educational constraints by offering a flexible, peer-driven learning environment that emphasizes practical experience and collaboration.

> **Note**: The application is still a work in progress and not complete nor production ready.

## 🚀 Project Structure

```ts
NXTApp/
├── backend/              # .NET Core backend
│   ├── NXTBackend.API   # Main API project
│   ├── Core             # Core business logic
│   └── Infrastructure   # Data access, external services
└── frontend/            # SvelteKit frontend
    ├── src/             # Source code
    └── static/          # Static assets


# 🛠️ Technology Stack
## Backend
- .NET 8.0
- Entity Framework Core
- OpenAPI/Swagger
- JWT Authentication

## Frontend
- SvelteKit
- TypeScript
- OpenAPI Client
- TailwindCSS

# 🚦 Getting Started
## Prerequisites
- .NET 8.0 SDK
- Bun Runtime
- PostgreSQL
- Minio (for self-hosted S3 storage)
- Keycloak

## Environment Variables

```env
# Keycloak Configuration
KC_CLIENT_ID="intra"
KC_CLIENT_SECRET="..."
KC_ISSUER="..."
KC_COOKIE_NAME="kc.session"

# Application
APP_NAME="App"

# S3 Storage (Minio)
S3_BUCKET="..."
S3_ACCESS_KEY_ID="..."
S3_SECRET_ACCESS_KEY="..."
S3_ENDPOINT="..."
```

## Backend
```sh
cd /Users/wizard/Developer/NXTApp/backend
dotnet restore
dotnet build
dotnet run --project "NXTBackend.API" --launch-profile "http"
```

### Migrations

```sh
cd /Users/wizard/Developer/NXTApp/backend
dotnet ef migrations add <MigrationName> --project "NXTBackend.API.Infrastructure" -s "NXTBackend.API"
dotnet ef database update --project "NXTBackend.API.Infrastructure" -s "NXTBackend.API"
```

## Frontend

```sh
cd /Users/wizard/Developer/NXTApp/frontend
bun install
bun --bun run dev
```


# 🚀 Deployment
The recommended deployment method is via Coolify. This provides:

- Automatic deployments from Git
- Environment variable management
- Container orchestration
- SSL certificate management

## Minio Setup
- Deploy Minio instance
- Create bucket and access credentials
- Configure S3 environment variables
