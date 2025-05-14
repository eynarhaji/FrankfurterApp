using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace FrankfurterApp.ErrorHandling.SwaggerSamples
{
    public class ValidationErrorSwaggerExample : BadRequestErrorSwaggerExample
    {
        public override IEnumerable<SwaggerExample<ErrorResponse>> GetExamples()
        {
            foreach (SwaggerExample<ErrorResponse> s in base.GetExamples())
            {
                yield return s;
            }
            
            yield return SwaggerExample.Create(
                "Validation Error",
                new ErrorResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "PropertyNamePascalCase", 
                        new List<string> { 
                            "Validation error message 1", 
                            "Validation error message 2",
                            "and etc." 
                            } 
                        }
                    },
                    CorrelationId = Guid.NewGuid().ToString(),
                    Status = System.Net.HttpStatusCode.BadRequest
                });
        }
    }
}