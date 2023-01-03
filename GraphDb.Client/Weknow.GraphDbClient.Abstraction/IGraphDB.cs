namespace Weknow.GraphDbClient.Abstraction;

public interface IGraphDB: IGraphDBRunner
{

    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <returns></returns>
    IGraphDBTransaction StartTransaction(TimeSpan? timeout = null);

    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    IGraphDBTransaction StartTransaction(GraphDBTransactionConfig configuration);
}
