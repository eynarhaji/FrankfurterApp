using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.ErrorHandling.SwaggerSamples
{
    public class GenericErrorSwaggerExample : IMultipleExamplesProvider<ErrorResponse>
    {
        public IEnumerable<SwaggerExample<ErrorResponse>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Generic Internal Server Error",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Error", new List<string> { "Error message" } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.InternalServerError
                });
        }
    }
}