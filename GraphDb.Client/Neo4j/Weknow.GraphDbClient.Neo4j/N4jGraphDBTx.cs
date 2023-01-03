using Neo4j.Driver;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;


namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal sealed partial class N4jGraphDBTx : IGraphDBTransaction
{
    private Task<IAsyncTransaction> _tx;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    /// <param name="configuration">The configuration.</param>
    public N4jGraphDBTx(IAsyncSession session, GraphDBTransactionConfig configuration)
    {
        _tx =  session.BeginTransactionAsync(c =>
        {
            TimeSpan? timeout = configuration?.Timeout;
            if (timeout != null)
            {
                c.WithTimeout(timeout);
            }
        });
    }

    #endregion // Ctor

    #region RunAsync

    /// <summary>
    /// Executes Cypher
    /// </summary>
    /// <param name="cypherCommand">The cypher command.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>
    /// Response factory
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    async ValueTask<IGraphDBResponse> IGraphDBRunner.RunAsync(CypherCommand cypherCommand, CypherParameters? parameters)
    {
        var tx = await _tx;
        IResultCursor cursor = await tx.RunAsync(cypherCommand, parameters ?? cypherCommand.Parameters);
        return await GraphDBResponse.Create(cursor);
    }

    #endregion // RunAsync

    #region CommitAsync

    /// <summary>
    /// Asynchronously commit this transaction.
    /// </summary>
    /// <returns></returns>
    async Task IGraphDBTransaction.CommitAsync()
    {
        var tx = await _tx;
        await tx.CommitAsync();
    }

    #endregion // CommitAsync

    #region RollbackAsync

    /// <summary>
    /// Asynchronously roll back this transaction.
    /// </summary>
    /// <returns></returns>
    async Task IGraphDBTransaction.RollbackAsync()
    {
        var tx = await _tx;
        await tx.RollbackAsync();
    }

    #endregion // RollbackAsync

    #region Dispose pattern

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous dispose operation.
    /// </returns>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        var tx = await _tx;
        await tx.DisposeAsync();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="N4jGraphDBTx"/> class.
    /// </summary>
    ~N4jGraphDBTx()
    {
        (this as IAsyncDisposable).DisposeAsync().GetAwaiter().GetResult();
    }

    #endregion // Dispose pattern
}
