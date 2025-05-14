using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FrankfurterApp.ErrorHandling.Exceptions;
using FrankfurterApp.ExecutionContext;
using FrankfurterApp.Extensions;
using FrankfurterApp.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrankfurterApp.ErrorHandling;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
    private readonly ILanguageTranslator _translator;

    public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger, ILanguageTranslator translator)
    {
        _next = next;
        _logger = logger;
        _translator = translator;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        
        HttpStatusCode status = HttpStatusCode.BadRequest;
        Dictionary<string, List<string>> message = new();

        try
        {
            await _next(context);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add("", [$"{_translator.Translate(ex.Message)} Status: {ex.StatusCode}"]);
            response.StatusCode = (int)status;

            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add("", [_translator.Translate(ex.Message)]);
            response.StatusCode = (int)status;

            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
        catch (BusinessLogicException ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add(ex.Key, [string.Format(_translator.Translate(ex.Message), ex.Argument)]);
            response.StatusCode = (int)status;
            
            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add(LocalizationStrings.UnathorizedException, [_translator.Translate(LocalizationStrings.UnathorizedException)]);
            status = HttpStatusCode.Unauthorized;
            response.StatusCode = (int)status;

            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
        catch (ForbiddenException ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add(LocalizationStrings.ForbiddenException, [_translator.Translate(LocalizationStrings.ForbiddenException)]);
            status = HttpStatusCode.Forbidden;
            response.StatusCode = (int)status;

            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception - {Exception}", ex.ToString());

            message.Add(LocalizationStrings.InternalServerErrorException,
                [_translator.Translate(LocalizationStrings.InternalServerErrorException)]);
            status = HttpStatusCode.InternalServerError;
            response.StatusCode = (int)status;
            
            await response.WriteAsync(new ErrorResponse
                    { Errors = message, CorrelationId = ExecutionContextAccessor.GetCorrelationId(), Status = status }
                .JsonSerializeToSnakeCase());
        }
    }
}