internal class RequestLoggingMiddleware
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
        // Log request details
        _logger.LogInformation("Received Request: {Method} {Path} from {IPAddress}",
            context.Request.Method, context.Request.Path, context.Connection.RemoteIpAddress);

        // Call the next middleware in the pipeline
        await _next(context);
    }
}