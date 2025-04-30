using Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred.");

        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();
        var statusCode = HttpStatusCode.InternalServerError; // Default 500

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                response.Errors = validationException.Errors;
                response.Message = "One or more validation errors occurred.";
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                response.Message = "Unauthorized access.";
                break;

            case ForbiddenAccessException:
                statusCode = HttpStatusCode.Forbidden;
                response.Message = "Forbidden access.";
                break;

            default:
                response.Message = "Internal server error.";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(result);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public object? Errors { get; set; }
    }
}
