namespace UserManagementAPI.Middleware;

internal static class RequestAuditLog
{
    public static void Write(
        ILogger logger,
        HttpContext context,
        int statusCode,
        double elapsedMilliseconds)
    {
        logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds:F2} ms. TraceId: {TraceId}",
            context.Request.Method,
            context.Request.Path,
            statusCode,
            elapsedMilliseconds,
            context.TraceIdentifier);
    }
}
