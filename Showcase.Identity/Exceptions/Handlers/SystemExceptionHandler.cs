using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Showcase.Identity.Result;

namespace Showcase.Identity.Exceptions.Handlers;

public class SystemExceptionHandler(ILogger logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var exceptionBody = ServiceResult.SystemError(exception.ToExceptionViewModel());

        var body = JsonSerializer.Serialize(exceptionBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        logger.LogCritical(exception, "SystemException occurred with the traceId: {TraceId}", exceptionBody.ErrorResult?.Data?.TraceId);
        
        await httpContext.Response.WriteAsync(body, cancellationToken);
        return true;
    }
}