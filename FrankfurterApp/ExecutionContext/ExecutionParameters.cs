using System;

namespace Turn.Application.ExecutionContext;

public class ExecutionParameters
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    public string CurrentAppName { get; set; }
    public string PreviousAppName { get; set; }
    public string ClientId { get; set; }
    public string IpAddress { get; set; }
}