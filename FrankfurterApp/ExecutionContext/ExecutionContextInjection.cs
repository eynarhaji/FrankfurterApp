using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.ExecutionContext;

public static class ExecutionContextInjection
{
    public static IServiceCollection AddExecutionContext(this IServiceCollection services)
    {
        services.AddScoped<ExecutionContextHttpRequestHandler>();

        return services;
    }
    
    public static IApplicationBuilder AddExecutionContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExecutionContextMiddleware>();
        
        return app;
    }
}