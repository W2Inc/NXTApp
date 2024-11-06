// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Reflection;
using System.Text.Json;
using System.Threading.RateLimiting;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Infrastructure.Interceptors;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

// ============================================================================

namespace NXTBackend.API;

public static class Startup
{
    /// <summary>
    /// Handles the registration of services and other configurations
    /// </summary>
    public static void RegisterServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidDataException("Connection string not found in appsettings.json");

        // API and JSON settings
        services.AddEndpointsApiExplorer();
        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

        // Authentication and Authorization (Keycloak)
        services.AddKeycloakWebApiAuthentication(builder.Configuration);
        services.AddAuthorizationBuilder().AddPolicy("admin", b => b.RequireRole("admin"));

        // Swagger / OpenAPI Configuration
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NXTBackend",
                Version = "0.0.1-alpha",
                Description = "The NXTBackend is the public backend used for handling all this stuff.",
                Contact = new OpenApiContact
                {
                    Email = "info@nextdemy.com",
                    Name = "W2Wizard"
                }
            });

            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            var options = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Auth",
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{options.KeycloakUrlRealm}protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{options.KeycloakUrlRealm}protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>()
                    }
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
        });

        // Database Context and Seeders
        services.AddDbContext<DatabaseContext>((sp, options) =>
        {
            options.AddInterceptors(new SavingChangesInterceptor(sp.GetRequiredService<TimeProvider>()));
            options.UseLazyLoadingProxies().UseNpgsql(connectionString);
        })
        .AddTransient<DatabaseSeeder>();

        // Caching Configuration
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(30)));
            options.AddPolicy("NoCache", builder => builder.NoCache());
            options.AddPolicy("1m", b => b.Expire(TimeSpan.FromSeconds(60)));
            options.AddPolicy("2m", b => b.Expire(TimeSpan.FromSeconds(120)));
        });
        services.AddDistributedMemoryCache();

        // Dependency Injection for Services
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFeatureService, FeatureService>();
        services.AddScoped<INotificationService, NotifcationService>();
        services.AddSingleton(TimeProvider.System);

        // Rate Limiting
        services.AddRateLimiter(limiter =>
        {
            limiter.RejectionStatusCode = 429;
            limiter.AddFixedWindowLimiter("fixed", options =>
            {
                options.PermitLimit = 10;
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 5;
            });
        });

        // CORS Policy
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        // Serilog Logging
        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(
                "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                theme: TemplateTheme.Code
            )));
    }
}
