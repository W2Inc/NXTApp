# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy the solution file and all project files into the container
COPY NXTBackend.sln NXTBackend.sln
COPY NXTBackend.API/NXTBackend.API.csproj NXTBackend.API/NXTBackend.API.csproj
COPY NXTBackend.API.Core/NXTBackend.API.Core.csproj NXTBackend.API.Core/NXTBackend.API.Core.csproj
COPY NXTBackend.API.Domain/NXTBackend.API.Domain.csproj NXTBackend.API.Domain/NXTBackend.API.Domain.csproj
COPY NXTBackend.API.Models/NXTBackend.API.Models.csproj NXTBackend.API.Models/NXTBackend.API.Models.csproj
COPY NXTBackend.API.Infrastructure/NXTBackend.API.Infrastructure.csproj NXTBackend.API.Infrastructure/NXTBackend.API.Infrastructure.csproj

# Restore dependencies as a separate layer
RUN dotnet restore

# Copy the remaining application source code
COPY . .

# Build and publish the application to the /app directory
RUN dotnet publish -c Release -o /app --no-restore

# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "NXTBackend.API.dll"]
