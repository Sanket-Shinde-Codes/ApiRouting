using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Middleware;

internal sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var isBadRequest = exception is BadHttpRequestException or JsonException;
        var status = isBadRequest
            ? StatusCodes.Status400BadRequest
            : StatusCodes.Status500InternalServerError;
        var title = isBadRequest ? "Bad Request" : "An unexpected error occurred";
        var detail = isBadRequest
            ? "The request body is malformed or could not be read."
            : "The server could not complete the request.";

        if (isBadRequest)
        {
            logger.LogInformation(exception, "The request body could not be parsed.");
        }
        else
        {
            logger.LogError(exception, "An unhandled exception occurred while processing the request.");
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}
