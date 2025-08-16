using Ambev.Ominia.Domain.Exceptions;
using Ambev.Ominia.Domain.Responses;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Ambev.Ominia.Crosscutting.Validation;
using Microsoft.Extensions.Logging;

namespace Ambev.Ominia.Crosscutting.Middleware;

public class ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new Response
        {
            Success = false,
            Message = "Validation Failed",
            Errors = exception.Failures.Select(error => (ValidationErrorDetail)error)
        };

        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }

    private static Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        var response = new
        {
            Success = false,
            exception.Message
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedAccessException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        var response = new
        {
            Success = false,
            exception.Message
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request. Path: {Path}, Method: {Method}", 
            context.Request.Path, context.Request.Method);
            
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            Success = false,
            Message = "An error occurred while processing your request.",
            Details = exception.Message // Include exception details in development
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}