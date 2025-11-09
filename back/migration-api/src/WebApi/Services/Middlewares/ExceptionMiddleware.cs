using Application.Dependencies.Logging;
using CustomerMigrationApi.Services.Middlewares.Dtos;
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, Guid.NewGuid());
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, Guid id)
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
}