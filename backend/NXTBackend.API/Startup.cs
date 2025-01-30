// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using NXTBackend.API.Jobs;
using NXTBackend.API.Jobs.Interface;
using NXTBackend.API.Utils;
using Quartz;
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
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<ServiceExceptionFilter>();
            options.InputFormatters.Add(new TextPlainInputFormatter());
        });

        // Authentication and Authorization (Keycloak)
        services.AddKeycloakWebApiAuthentication(builder.Configuration);
        services.AddAuthorizationBuilder().AddPolicy("admin", b => b.RequireRole("admin"));

        // Swagger / OpenAPI Configuration
        services.AddOpenApi(o =>
        {
            // Add Document Transformers
            o.AddDocumentTransformer<InfoSchemeTransformer>();
            o.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            o.AddOperationTransformer<BasicResponsesOperationTransformer>();

            // Keycloak Authentication
            o.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                // TODO: Get from config
                if (builder.Environment.IsDevelopment())
                    document.Servers = [new() { Url = "http://localhost:3001" }];
                document.Components ??= new OpenApiComponents();

                var options = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;
                document.Components.SecuritySchemes.Add("OAuth2", new OpenApiSecurityScheme
                {
                    Name = "Keycloak Server",
                    OpenIdConnectUrl = new Uri($"{options.KeycloakUrlRealm}protocol/openid-connect"),
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{options.KeycloakUrlRealm}protocol/openid-connect/auth"),
                            TokenUrl = new Uri($"{options.KeycloakUrlRealm}protocol/openid-connect/token"),
                        }
                    }
                });

                return Task.CompletedTask;
            });
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
            options.AddPolicy("1m", b => b.Expire(TimeSpan.FromMinutes(1)));
            options.AddPolicy("2m", b => b.Expire(TimeSpan.FromMinutes(2)));
            options.AddPolicy("1h", b => b.Expire(TimeSpan.FromHours(1)));
        });
        services.AddDistributedMemoryCache();
        services.AddResponseCompression();

        // Dependency Injection for Services
        services.AddScoped<ICursusService, CursusService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserCursusService, UserCursusService>();
        services.AddScoped<IUserGoalService, UserGoalService>();
        services.AddScoped<IUserProjectService, UserProjectService>();
        services.AddScoped<IFeatureService, FeatureService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IRubricService, RubricService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISpotlightEventService, SpotlightEventService>();
        services.AddScoped<IGitService, GitService>();
        services.AddScoped<ISpotlightEventActionService, SpotlightEventActionService>();
        services.AddSingleton(TimeProvider.System);

        // Rate Limiting
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429;
            options.AddPolicy("DynamicPolicy", context =>
            {
                if (!(context.User?.Identity?.IsAuthenticated ?? false))
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: "UnauthenticatedUsers",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 60, // Fewer requests
                            Window = TimeSpan.FromMinutes(20),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        });
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "AuthenticatedUsers",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 240, // More requests
                        Window = TimeSpan.FromMinutes(20),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5
                    });
            });
        });


        // CORS Policy
        // if (!builder.Environment.IsDevelopment())
        // {
        //     services.AddCors(options =>
        //     {
        //         options.AddPolicy("AllowSpecificOrigin", builder =>
        //         {
        //             builder.WithOrigins("https://localhost:5173")
        //                 .AllowAnyMethod()
        //                 .AllowAnyHeader()
        //                 .AllowCredentials();
        //         });
        //     });
        // }

        // Serilog Logging
        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(
                "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                theme: TemplateTheme.Code
            )));

        builder.Services.AddQuartz(q =>
        {
            q.SchedulerName = "NXT";
            q.SchedulerId = "Queue";
            q.UseDefaultThreadPool(x => x.MaxConcurrency = 5);

            RegisterJob<ReviewCompositionJob>(q);
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }


    public static void RegisterJob<Job>(IServiceCollectionQuartzConfigurator quartz) where Job : IScheduledJob
    {
        try
        {
            JobKey jobKey = new(Job.Identity);
            quartz.AddJob<Job>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts =>
            {
                opts.ForJob(jobKey);
                opts.WithIdentity($"{Job.Identity}-trigger");
                if (Job.Schedule is not null)
                    opts.WithCronSchedule(Job.Schedule);
            });

        }
        catch (FormatException)
        {
            Log.Error($"Failed to register job: {Job.Identity} due to badly formated cron: '{Job.Schedule}'");
            throw;
        }
    }
}
