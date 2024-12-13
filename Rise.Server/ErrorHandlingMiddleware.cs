using System.Net;
using System.Text.Json;
using Rise.Shared.Exceptions;
using Serilog;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = context.Response;
        var errorResponse = new ErrorResponse
        {
            Message = exception.Message,
            Details = exception.Message
        };

        switch (exception)
        {
            case NotFoundException _:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = "The requested item was not found.";
                break;
            case BadRequestException _:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "The request was invalid.";
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred.";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        return context.Response.WriteAsync(result);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
    }
}