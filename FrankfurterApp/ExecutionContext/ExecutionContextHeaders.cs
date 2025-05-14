namespace Turn.Application.ExecutionContext;

public static class ExecutionContextHeaders
{
    public const string CorrelationId = "CorrelationId";
    public const string CurrentAppName = "CurrentAppName";
    public const string PreviousAppName = "PreviousAppName";
    public const string ClientId = "client_id";
    public const string IpAddress = "X-Forwarded-For";
}