using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.ErrorHandling.SwaggerSamples
{
    public class UnauthorizedErrorSwaggerExample : IMultipleExamplesProvider<ErrorResponse>
    {
        public IEnumerable<SwaggerExample<ErrorResponse>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Authorization Error",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Authorization", new List<string> { "Authorization error message." } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.Unauthorized
                });
        }
    }
}