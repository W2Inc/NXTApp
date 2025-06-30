# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy the entire solution
COPY . .

# Restore NuGet packages properly for all projects
RUN dotnet restore NXTBackend.sln

# Build the application (build projects individually to avoid the solution output issue)
RUN dotnet build NXTBackend.sln -c Release --no-restore

# Publish the main API project
RUN dotnet publish NXTBackend.API/NXTBackend.API.csproj -c Release -o /app --no-build

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

EXPOSE 3001
ENTRYPOINT ["dotnet", "NXTBackend.API.dll"]
