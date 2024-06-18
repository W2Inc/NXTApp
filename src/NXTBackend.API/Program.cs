using Microsoft.AspNetCore.Authentication;
using NXTBackend.API;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");
var builder = WebApplication.CreateBuilder(args);
Startup.RegisterServices(builder);

var app = builder.Build();
if (!await app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseSeeder>().InitializeAsync(builder.Configuration, args))
    throw new HostAbortedException();

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
app.Run();