# NXT Application

A modern, self-hostable, and fully open-source education platform designed to change how we approach learning. NXT aims to break free from traditional educational constraints by offering a flexible, peer-driven learning environment that emphasizes practical experience and collaboration.

> **Note**: The application is still a work in progress and not complete nor production ready.

## 🚀 Project Structure

```ts
NXTApp/
├── backend/                           # .NET Core backend
│   ├── NXTBackend.API                 # Controllers, Ratelimiter, Middleware, ...
│   ├── NXTBackend.API.Core            # Core business logic
│   ├── NXTBackend.API.Domain          # Models for the database
│   ├── NXTBackend.API.Infrastructure  # Database setup, migrations, contexts, ...
│   └── NXTBackend.API.Model           # Request and response models (DO's, DTO's, ...)
└── frontend/            # SvelteKit frontend
    ├── src/             # Source code
    └── static/          # Static assets
```

# 🛠️ Technology Stack
## Backend
- .NET 9.0
- Entity Framework Core
- OpenAPI/Swagger
- Keycloak Authentication

## Frontend
- SvelteKit
- Bun
- TypeScript
- OpenAPI Client
- TailwindCSS

# 🚦 Getting Started
## Prerequisites
- .NET 9.0 SDK
- Bun Runtime
- PostgreSQL @ 16.0
- Minio (for self-hosted S3 storage)
- Keycloak @ 26.0.0

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
The recommended deployment method is via Coolify. Coolify is an open source self hostable 'vercel' essentially that gives you a several templates to spin up services.

This provides:
- Automatic deployments from Git
- Environment variable management
- Container orchestration
- SSL certificate management
- Backups
- ...
  

## Minio Setup
- Deploy Minio instance
- Create bucket and access credentials
- Configure S3 environment variables
