using System.Net;
using Events.Core.Entities.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string Title = "Unexpected error.";
        HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
        string Message = "An unexpected error occurred.";
        
        if (exception is ValidationException)
        {
            Title = "Validation errors occured.";
            StatusCode = HttpStatusCode.BadRequest;
            Message = $"Validation errors: {exception.Message}";
        }
        if (exception is NotFoundException)
        {
            Title = "Not found error occured.";
            StatusCode = HttpStatusCode.NotFound;
            Message = $"NotFound: {exception.Message}";
        }
        else if (exception is ValidationException)
        {
            Title = "Validation error occurred.";
            StatusCode = HttpStatusCode.BadRequest;
            Message = $"Validation failed: {exception.Message}";
        }
        else if (exception is UnauthorizedAccessException)
        {
            Title = "Unauthorized error occurred.";
            StatusCode = HttpStatusCode.Unauthorized;
            Message = "You are not authorized to access this resource.";
        }
        else if (exception is DbUpdateException)
        {
            Title = "Database error occurred.";
            StatusCode = HttpStatusCode.InternalServerError;
            Message = $"A database error occurred: {exception.Message}";
        }
        else if (exception is ArgumentException)
        {
            Title = "Argument error occurred.";
            StatusCode = HttpStatusCode.BadRequest;
            Message = $"Invalid argument: {exception.Message}";
        }
        else if (exception is TimeoutException)
        {
            Title = "Timeout error occurred.";
            StatusCode = HttpStatusCode.RequestTimeout;
            Message = "The request has timed out. Please try again later.";
        }
        else if (exception is UnauthorizedOperationException) 
        {
            Title = "Unauthorized operation exception.";
            StatusCode = HttpStatusCode.Forbidden;
            Message = exception.Message;
        }

        var response = new
        {
            Title,
            StatusCode = (int)StatusCode,
            Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)StatusCode;
        var jsonResponse = JsonConvert.SerializeObject(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}
