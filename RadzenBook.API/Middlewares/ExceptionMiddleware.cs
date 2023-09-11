﻿using System.Text.Json;
using RadzenBook.Common.Exceptions;

namespace RadzenBook.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env, RequestDelegate next)
    {
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            RepositoryException => StatusCodes.Status500InternalServerError,
            ServiceException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = _env.IsDevelopment()
            ? new AppException(httpContext.Response.StatusCode, exception.Message, exception.StackTrace!.Trim(), exception.GetType().ToString(), httpContext.TraceIdentifier)
            : new AppException(StatusCodes.Status500InternalServerError, "Internal Server Title", "An internal error occurred. Please contact support for assistance.", traceId: httpContext.TraceIdentifier, type: "https://tools.ietf.org/html/rfc7231#section-6.6.1");

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}