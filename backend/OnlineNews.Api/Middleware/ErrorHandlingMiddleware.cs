using System.Net;
using System.Text.Json;

namespace OnlineNews.Api.Middleware;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

/// <summary>Turns exceptions into consistent JSON error responses.</summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var status = ex switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
            if (status == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Unhandled error");

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";
            var body = new
            {
                timestamp = DateTime.Now,
                status = (int)status,
                message = status == HttpStatusCode.InternalServerError ? "Something went wrong" : ex.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
