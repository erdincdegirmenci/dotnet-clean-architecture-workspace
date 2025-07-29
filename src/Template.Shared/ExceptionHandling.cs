using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Template.Shared;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) => new() { Success = true, Data = data, Message = message };
    public static ApiResponse<T> FailResponse(string message, List<string>? errors = null) => new() { Success = false, Message = message, Errors = errors };
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = _env.IsDevelopment()
                ? ApiResponse<string>.FailResponse(ex.Message, new List<string> { ex.StackTrace ?? string.Empty })
                : ApiResponse<string>.FailResponse("Internal Server Error");
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
} 