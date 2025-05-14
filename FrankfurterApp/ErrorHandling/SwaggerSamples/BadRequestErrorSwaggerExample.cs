using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.ErrorHandling.SwaggerSamples
{
    public class BadRequestErrorSwaggerExample : IMultipleExamplesProvider<ErrorResponse>
    {
        public virtual IEnumerable<SwaggerExample<ErrorResponse>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Generic Error",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Error", new List<string> { "Error message" } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.BadRequest
                });
            
            yield return SwaggerExample.Create(
                "Error with Error Code",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Snake_Case_Error_Code_With_Capital", new List<string> { "Error message" } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.BadRequest
                });
            
            yield return SwaggerExample.Create(
                "Error with multi-line message",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Snake_Case_Error_Code_With_Capital", new List<string> { "In this error message/nfrom here it should be new line." } }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.BadRequest
                });
        }
    }
}