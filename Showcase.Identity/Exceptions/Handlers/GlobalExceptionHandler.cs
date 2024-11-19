using Microsoft.AspNetCore.Diagnostics;

namespace Showcase.Identity.Exceptions.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return exception switch
        {
            ShowcaseDatabaseException => await new DatabaseExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            ShowcaseUnauthorizedException => await new UnauthorizedExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            ShowcaseNotFoundException => await new NotFoundExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            ShowcaseForbiddenException => await new ForbiddenExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            ShowcaseOperationalException => await new OperationalExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            ShowcaseBusinessException => await new BusinessExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken),
            _ => await new SystemExceptionHandler(logger).TryHandleAsync(httpContext, exception, cancellationToken)
        };
    }
}