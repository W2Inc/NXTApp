// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API;
using Serilog;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use all the middleware
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("fixed");

// Shortcut
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/docs")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next(context);
    });
}

app.Run();