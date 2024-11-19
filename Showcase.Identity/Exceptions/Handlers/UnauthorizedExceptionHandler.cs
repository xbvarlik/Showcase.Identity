using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Showcase.Identity.Result;

namespace Showcase.Identity.Exceptions.Handlers;

public class UnauthorizedExceptionHandler(ILogger logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        httpContext.Response.ContentType = "application/json";
        
        var exceptionBody = ServiceResult.Unauthorized(exception.ToExceptionViewModel());
        
        var body = JsonSerializer.Serialize(exceptionBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        logger.LogWarning(exception, "UnauthorizedException occurred with the traceId: {TraceId}", exceptionBody.ErrorResult?.Data?.TraceId);
        
        await httpContext.Response.WriteAsync(body, cancellationToken);
        return true;
    }
}