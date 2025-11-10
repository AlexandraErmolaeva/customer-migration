using Application.Dependencies.Logging;
using CustomerMigrationApi.Services.Middlewares.Dtos;
using FluentValidation;
using System.Net;

namespace CustomerMigrationApi.Services.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
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
        catch (ValidationException ex)
        {
            // Перехватываем ошибку валидации для показа модалки на фронте.
            await HandleValidationExceptionAsync(httpContext, ex, Guid.NewGuid());
        }
        catch (Exception ex)
        {
            // Перехватывем общую ошибку для показа модалки на фронте и пишем в лог.
            await HandleGeneralExceptionAsync(httpContext, ex, Guid.NewGuid());
        }
    }

    private async Task HandleGeneralExceptionAsync(HttpContext context, Exception exception, Guid id)
    {
        _logger.LogError($"{id} - {exception.ToString()}");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ExceptionDetails()
        {
            Id = id,
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        }.ToString());
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, Exception exception, Guid id)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ExceptionDetails()
        {
            Id = id,
            StatusCode = context.Response.StatusCode,
            Message = "Ошибка валидации данных: " + exception.Message
        }.ToString());
    }
}