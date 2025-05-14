using System;
using FrankfurterApp.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace FrankfurterApp.Logging;

public static class SerilogInjection
{
    public static IHostBuilder AddSerilog(this IHostBuilder host, IConfiguration configuration)
    {
        var settings = configuration.ExtractSettings<SerilogSettings>("Serilog");
        
        var serilogConfiguration = new LoggerConfiguration()
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(settings.RestrictionLevel, true))
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {CorrelationId} {PreviousAppName} {CurrentAppName} {UserId} {SchemaName} {Message:lj}{NewLine}{Exception}");

        if (settings.ForceElasticsearch)
        {
            serilogConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(settings.NodeUri))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                IndexFormat = $"{settings.IndexFormat}-{DateTime.Today:yyyy-MM}",
                NumberOfReplicas = int.Parse(settings.NumberOfReplicas),
                ModifyConnectionSettings = connectionConfiguration =>
                {
                    if (settings.ForceAuthentication)
                        connectionConfiguration.BasicAuthentication(settings.Username, settings.Password);

                    return connectionConfiguration.ServerCertificateValidationCallback((_, _, _, _) => true);
                }
            });
        }
        
        host.ConfigureLogging((_, _) =>
        {
            Log.Logger = serilogConfiguration.CreateLogger();
        }).UseSerilog();

        return host;
    }
}