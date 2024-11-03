// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using NXTBackend.API.Core.Services.Interface;

// ============================================================================

namespace NXTBackend.API.Middleware;

/// <summary>
///
/// </summary>
/// <param name="next">The next request</param>
/// <param name="log">Logging service</param>
public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.GetUserId();
        log.LogInformation("[RequestLogMiddleware] Request => {Path} from {@id}", context.Request.Path, id?.ToString() ?? "Unknown");
        await next(context);
    }
}
