using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.ErrorHandling.SwaggerSamples
{
    public class ForbiddenErrorSwaggerExample : IMultipleExamplesProvider<ErrorResponse>
    {
        public IEnumerable<SwaggerExample<ErrorResponse>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Authentication Error",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Authentication", new List<string> { "Authentication error message." } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.Forbidden
                });
        }
    }
}