using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Turn.Application.ExecutionContext;

namespace FrankfurterApp.ExecutionContext;

public class ExecutionContextHttpRequestHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(ExecutionContextHeaders.CorrelationId, ExecutionContextAccessor.GetCorrelationId());
        request.Headers.Add(ExecutionContextHeaders.CurrentAppName, ExecutionContextAccessor.GetCurrentAppName());

        return base.SendAsync(request, cancellationToken);
    }
}