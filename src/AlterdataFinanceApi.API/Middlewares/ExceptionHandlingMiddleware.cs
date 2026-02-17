using System.Net;
using System.Text.Json;
using FluentValidation;

namespace AlterdataFinanceApi.API.Middlewares;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Message = "Erro de validação.",
                    Errors = validationEx.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    })
                } as object
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Message = exception.Message
                } as object
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Message = exception.Message
                } as object
            ),
            InvalidOperationException => (
                HttpStatusCode.Conflict,
                new
                {
                    Status = (int)HttpStatusCode.Conflict,
                    Message = exception.Message
                } as object
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Message = "Ocorreu um erro interno no servidor."
                } as object
            )
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
