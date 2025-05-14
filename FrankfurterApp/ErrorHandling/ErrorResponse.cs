using System.Collections.Generic;
using System.Net;

namespace FrankfurterApp.ErrorHandling
{
    /// <summary>
    ///     Author: Eynar Haji
    ///     Description:
    ///     DTO to handle errors.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        ///     Error messages.
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        ///     Url of error type.
        /// </summary>
        public string CorrelationId { get; set; } = "";

        /// <summary>
        ///     Status code of error.
        /// </summary>
        public HttpStatusCode Status { get; set; }
    }
}