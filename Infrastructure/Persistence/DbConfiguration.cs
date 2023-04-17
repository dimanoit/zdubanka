namespace Infrastructure.Persistence;

public record DbConfiguration
{
    public string Host { get; init; } = null!;

    public string Name { get; init; } = null!;

    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
    public int Port { get; init; }
    public int Timeout { get; init; }
    public int CommandTimeout { get; init; }
    public int RetryCount { get; init; }
    public int RetryTime { get; init; }
    public bool Pooling { get; init; } = true;
}