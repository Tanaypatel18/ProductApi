namespace myFirstWebApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // before request hits controller
        _logger.LogInformation("Request: {Method} {Path} started at {Time}",
            context.Request.Method,
            context.Request.Path,
            DateTime.UtcNow);

        await _next(context); // pass to next middleware

        // after response comes back
        _logger.LogInformation("Response: {StatusCode} finished at {Time}",
            context.Response.StatusCode,
            DateTime.UtcNow);
    }
}