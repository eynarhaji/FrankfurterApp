using System.Diagnostics;
using System.Threading.Tasks;
using FrankfurterApp.ExecutionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrankfurterApp.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var ipAddress = ExecutionContextAccessor.GetIpAddress();
            var clientId = ExecutionContextAccessor.GetClientId();
            _logger.LogInformation(
                "ClientIP: {IP}, ClientId: {ClientId}, Method: {Method}, Path: {Path}, StatusCode: {Status}, ResponseTime: {Time}ms",
                ipAddress,
                clientId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}