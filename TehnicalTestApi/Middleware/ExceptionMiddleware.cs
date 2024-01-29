using System.Net;
using System.Text.Json;

namespace TechnicalTestApi.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(exception: ex, message: "An error occurred: {Message}", ex.Message);

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = environment.IsDevelopment()
            ? new ApiException(httpContext.Response.StatusCode, exception.Message, exception.StackTrace)
            : new ApiException(httpContext.Response.StatusCode, "Internal server Error");

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, jsonOptions);

        await httpContext.Response.WriteAsync(json);
    }

    private record ApiException(int StatusCode, string? Message, string? Details = null);
}