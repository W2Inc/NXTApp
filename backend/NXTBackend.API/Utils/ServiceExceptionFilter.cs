using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NXTBackend.API.Core.Utils;

namespace NXTBackend.API.Utils;

public class ServiceExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ServiceException serviceException)
            return;

        // NOTE(W2): For some we want to not have a body.
        switch (serviceException.StatusCode)
        {
            case StatusCodes.Status403Forbidden:
                context.Result = new ForbidResult();
                break;
            case StatusCodes.Status404NotFound:
                context.Result = new NotFoundResult();
                break;
            default:
                var problemDetails = new ProblemDetails
                {
                    Type = $"https://http.cat/{serviceException.StatusCode}",
                    Title = serviceException.Message,
                    Detail = serviceException.Detail,
                    Status = serviceException.StatusCode,
                    Instance = context.HttpContext.Request.Path,
                    Extensions = new Dictionary<string, object?>
                    {
                        ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                    }
                };

                var objectResult = new ObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };

                context.Result = objectResult;
                break;
        }


        context.ExceptionHandled = true;
    }
}
