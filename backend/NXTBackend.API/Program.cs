// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API;
using NXTBackend.API.Middleware;
using Scalar.AspNetCore;
using Serilog;

// Setup
// ============================================================================

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");
var builder = WebApplication.CreateBuilder(args);
Startup.RegisterServices(builder);

var app = builder.Build();
if (!await app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseSeeder>().InitializeAsync(builder.Configuration, args))
    throw new HostAbortedException("Unable to seed, are the containers up?");

// Developer
// ============================================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(o =>
    {
        o.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "OAuth2",
            OAuth2 = new OAuth2Options
            {
                ClientId = "intra",
                Scopes = ["email", "roles", "openid"]
            }
        };
    });
    app.UseDeveloperExceptionPage();
}

// Middleware
// ============================================================================
app.UseResponseCompression();
app.UseOutputCache();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<KeycloakUserMiddlerware>();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("fixed");
app.Run();
