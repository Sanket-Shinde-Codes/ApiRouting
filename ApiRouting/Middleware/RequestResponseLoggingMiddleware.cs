using System.Diagnostics;
using System.Text.Json;

namespace UserManagementAPI.Middleware;

internal sealed class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var statusCode = StatusCodes.Status200OK;

        try
        {
            await next(context);
            statusCode = context.Response.StatusCode;
        }
        catch (Exception exception)
        {
            statusCode = exception is BadHttpRequestException or JsonException
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            RequestAuditLog.Write(logger, context, statusCode, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
