using System.Net;
using System.Text.Json;
using AddressBook.Application.Common.Exceptions;

namespace AddressBook.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (statusCode, title, errors) = exception switch
        {
            NotFoundException nfe =>
                (HttpStatusCode.NotFound, nfe.Message, (IDictionary<string, string[]>?)null),

            ValidationException ve =>
                (HttpStatusCode.BadRequest, "One or more validation errors occurred.", ve.Errors),

            ConflictException ce =>
                (HttpStatusCode.Conflict, ce.Message, null),

            ForbiddenAccessException =>
                (HttpStatusCode.Forbidden, "Access to this resource is forbidden.", null),

            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, "Authentication is required.", null),

            InvalidOperationException ioe =>
                (HttpStatusCode.BadRequest, ioe.Message, null),

            ArgumentException ae =>
                (HttpStatusCode.BadRequest, ae.Message, null),

            _ =>
                (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        context.Response.StatusCode = (int)statusCode;

        var response = errors is not null
            ? new { status = (int)statusCode, title, errors }
            : (object)new { status = (int)statusCode, title };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
