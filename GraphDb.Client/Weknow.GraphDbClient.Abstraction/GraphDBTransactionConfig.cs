namespace Weknow.GraphDbClient.Abstraction;

/// <summary>
/// Transaction config
/// </summary>
public record GraphDBTransactionConfig
{
    /// <summary>
    /// Timeout.
    /// </summary>
    public TimeSpan? Timeout { get; init; }
}

