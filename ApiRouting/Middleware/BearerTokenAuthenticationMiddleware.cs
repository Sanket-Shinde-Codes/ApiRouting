using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Middleware;

internal sealed class BearerTokenAuthenticationMiddleware(
    RequestDelegate next,
    IConfiguration configuration,
    ILogger<BearerTokenAuthenticationMiddleware> logger)
{
    private readonly string? _expectedToken = configuration["Authentication:BearerToken"];

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await next(context);
            return;
        }

        var authorization = context.Request.Headers.Authorization.ToString();
        var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorization["Bearer ".Length..].Trim()
            : string.Empty;

        if (!IsValidToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.WWWAuthenticate = "Bearer";
            await Results.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized",
                detail: "A valid bearer token is required.",
                instance: context.Request.Path)
                .ExecuteAsync(context);

            RequestAuditLog.Write(
                logger,
                context,
                StatusCodes.Status401Unauthorized,
                elapsedMilliseconds: 0);
            return;
        }

        context.User = new ClaimsPrincipal(new ClaimsIdentity("Bearer"));
        await next(context);
    }

    private bool IsValidToken(string token)
    {
        if (string.IsNullOrEmpty(_expectedToken) || string.IsNullOrEmpty(token))
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(token),
            Encoding.UTF8.GetBytes(_expectedToken));
    }
}
