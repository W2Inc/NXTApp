// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using System.Reflection;
using System.Threading.RateLimiting;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Infrastructure.Interceptors;
using System.Text.Json;

// ============================================================================

namespace NXTBackend.API;

public static class Startup
{
    /// <summary>
    /// Handles the registration of services and other configurations
    /// </summary>
    public static void RegisterServices(WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidDataException("Connection string not found in appsettings.json");

        var services = builder.Services;
        services.AddEndpointsApiExplorer();
        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // Add Keycloak authentication and role-based authorization
        services.AddKeycloakWebApiAuthentication(builder.Configuration);
        services.AddAuthorization(o => o.AddPolicy("dev", b =>
        {
            b.RequireRole("dev");
        }));

        // All sorts of swagger stuff
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {
                Title = "NXTBackend",
                Version = "0.0.1-alpha",
                Description = "The NXTBackend is the public backend used for handling all this stuff.",
                Contact = new OpenApiContact
                {
                    Email = "info@nextdemy.com",
                    Name = "W2Wizard"
                }
            });
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please provide JWT with bearer (Bearer {jwt token})",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new List<string>() }
            });
        });

        // Add database context and seed it along with any interceptors
        services.AddScoped<ISaveChangesInterceptor, SavingChangesInterceptor>();
        services.AddDbContext<DatabaseContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            // https://docs.microsoft.com/en-us/ef/core/querying/related-data/lazy
            options.UseLazyLoadingProxies().UseNpgsql(connectionString, options =>
            {
                // options.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name);
            });
        })
        .AddTransient<DatabaseSeeder>();

        // Add services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddSingleton(TimeProvider.System);
        services.AddRateLimiter(limiter =>
        {
            // TODO: Varying rate limits for custom clients
            // TODO: Varying rate limits depending on if user is authenticated or not
            limiter.RejectionStatusCode = 429;

            // General purpose rate limiter for all requests
            limiter.AddFixedWindowLimiter("fixed", options =>
            {
                options.PermitLimit = 10;
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 5;
            });
        });

        // Cors Policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
        });

        // Serilog for nice logging
        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(
                // Include trace and span ids when present.
                "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                theme: TemplateTheme.Code)));
    }
}
