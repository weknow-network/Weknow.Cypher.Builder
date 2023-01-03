using Neo4j.Driver;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;


namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal sealed partial class N4jGraphDBTx : IGraphDBTransaction
{
    private IAsyncTransaction _tx;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    public N4jGraphDBTx(IAsyncTransaction transaction)
    {
        _tx = transaction;
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
        IResultCursor cursor = await _tx.RunAsync(cypherCommand, parameters ?? cypherCommand.Parameters);
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
        await _tx.CommitAsync();
    }

    #endregion // CommitAsync

    #region RollbackAsync

    /// <summary>
    /// Asynchronously roll back this transaction.
    /// </summary>
    /// <returns></returns>
    async Task IGraphDBTransaction.RollbackAsync()
    {
        await _tx.RollbackAsync();
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
        await _tx.DisposeAsync();
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
