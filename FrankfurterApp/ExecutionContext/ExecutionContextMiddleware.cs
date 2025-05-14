using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Turn.Application.ExecutionContext;

namespace FrankfurterApp.ExecutionContext;

public class ExecutionContextMiddleware
{
    private readonly RequestDelegate _next;

    public ExecutionContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.SetExecutionParameters();

        context.Response.Headers.Append(ExecutionContextHeaders.CorrelationId,
            ExecutionContextAccessor.GetCorrelationId());

        await _next(context); 
    }
}