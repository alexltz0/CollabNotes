using System.Net;
using System.Text.Json;
using FluentValidation;

namespace CollabNotes.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse("Validation Failed",
                    validationEx.Errors.Select(e => e.ErrorMessage).ToArray())),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse("Not Found", [exception.Message])),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse("Unauthorized", [exception.Message])),

            InvalidOperationException => (
                HttpStatusCode.Conflict,
                new ErrorResponse("Conflict", [exception.Message])),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse("Internal Server Error", ["An unexpected error occurred."]))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Unhandled exception occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

public record ErrorResponse(string Title, string[] Errors);
