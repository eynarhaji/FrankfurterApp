using System;
using System.Threading.RateLimiting;
using Asp.Versioning;
using FrankfurterApp.Authentication;
using FrankfurterApp.Cache;
using FrankfurterApp.ErrorHandling;
using FrankfurterApp.ErrorHandling.Exceptions;
using FrankfurterApp.ExecutionContext;
using FrankfurterApp.Injections;
using FrankfurterApp.Localization;
using FrankfurterApp.Logging;
using FrankfurterApp.Services.CurrencyRates;
using FrankfurterApp.Services.Exchange;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

builder.Host.AddSerilog(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var key = ExecutionContextAccessor.GetIpAddress();
        
        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        throw new BusinessLogicException(LocalizationStrings.RateLimitExceededException,
            key: LocalizationStrings.RateLimitExceededException);
    };
});

builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddLocalization(builder.Configuration);

builder.Services.AddCache(builder.Configuration);
builder.Services.AddCurrencyRateService(builder.Configuration);
builder.Services.AddExchangeService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

app.AddSwagger();

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseRouting();

app.UseRequestLocalization();

app.UseMiddleware<RequestLoggingMiddleware>();
app.AddExecutionContext();
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();