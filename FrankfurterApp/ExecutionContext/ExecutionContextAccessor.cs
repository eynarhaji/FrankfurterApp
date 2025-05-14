using System;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Turn.Application.ExecutionContext;

namespace FrankfurterApp.ExecutionContext;

public static class ExecutionContextAccessor
{
    private static readonly AsyncLocal<ExecutionParameters> Context = new AsyncLocal<ExecutionParameters>();

    public static string GetCorrelationId() => Context?.Value?.CorrelationId ?? Guid.NewGuid().ToString();
    public static string GetCurrentAppName() => Context?.Value?.CurrentAppName;
    public static string GetPreviousAppName() => Context?.Value?.PreviousAppName;
    public static string GetIpAddress() => Context?.Value?.IpAddress;
    public static string GetClientId() => Context?.Value?.ClientId;

    public static void SetExecutionContext(ExecutionParameters executionParameters) =>
        Context.Value = executionParameters;

    public static void SetExecutionParameters(this HttpContext context)
    {
        var request = context.Request;
        var header = request.Headers;
        var correlationHeader = header[ExecutionContextHeaders.CorrelationId];
        var prevAppNameHeader = header[ExecutionContextHeaders.CurrentAppName];
        var ipAddressHeader = header[ExecutionContextHeaders.IpAddress];
        var clientIdHeader = header[ExecutionContextHeaders.ClientId];

        SetExecutionContext(new ExecutionParameters()
        {
            CorrelationId = correlationHeader.Count > 0 ? correlationHeader[0] : Guid.NewGuid().ToString(),
            PreviousAppName = prevAppNameHeader.Count > 0 ? prevAppNameHeader[0] : request.Host.Host,
            CurrentAppName = Assembly.GetEntryAssembly()?.GetName().Name,
            IpAddress = ipAddressHeader.Count > 0 ? ipAddressHeader[0] : request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? Guid.NewGuid().ToString(),
            ClientId = clientIdHeader.Count > 0 ? clientIdHeader[0] : Guid.NewGuid().ToString(),
        });

        PushExecutionParametersToLogContext();
    }

    private static void PushExecutionParametersToLogContext()
    {
        LogContext.PushProperty(ExecutionContextHeaders.CorrelationId, GetCorrelationId());
        LogContext.PushProperty(ExecutionContextHeaders.PreviousAppName, GetPreviousAppName());
        LogContext.PushProperty(ExecutionContextHeaders.CurrentAppName, GetCurrentAppName());
    }
}