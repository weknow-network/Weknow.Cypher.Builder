namespace Neo4j.Driver
{
    /// <summary>
    /// Common extensions for Neo4J ORM (Query and materialization)
    /// </summary>
    /// <seealso cref="System.IAsyncDisposable" />
    public static class DriverExtensions
    {
        ///// <summary>
        ///// Asynchronously run a cypher and return a task of result stream.
        ///// This method accepts a representing a Cypher cypher which will be
        ///// compiled into a query object that can be used to efficiently execute this
        ///// cypher multiple times. This method optionally accepts a set of parameters
        ///// which will be injected into the query object cypher by Graph Database.
        ///// </summary>
        ///// <param name="cypher">A Cypher cypher</param>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns></returns>
        //Task<ICypherRunnerResultCursor RunAsync(CypherCommand cypher,
        //    CancellationToken? cancellationToken = null);

        ///// <summary>
        ///// Asynchronously run a cypher and return a task of result stream.
        ///// This method accepts a representing a Cypher cypher which will be
        ///// compiled into a query object that can be used to efficiently execute this
        ///// cypher multiple times. This method optionally accepts a set of parameters
        ///// which will be injected into the query object cypher by Graph Database.
        ///// </summary>
        ///// <param name="cypher">The cypher.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns></returns>
        //Task<ICypherRunnerResultCursor> RunAsync(
        //    CypherCommand cypher,
        //    IDictionary<> parameters,
        //    CancellationToken? cancellationToken = null);

        ///// <summary>
        ///// Asynchronously run a cypher and return a task of result stream.
        ///// This method accepts a representing a Cypher cypher which will be
        ///// compiled into a query object that can be used to efficiently execute this
        ///// cypher multiple times. This method optionally accepts a set of parameters
        ///// which will be injected into the query object cypher by Graph Database.
        ///// </summary>
        ///// <param name="cypher">The cypher.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns></returns>
        //Task<ICypherRunnerResultCursor> RunAsync(
        //    CypherCommand cypher,
        //    IDictionary<string, object> parameters,
        //    CancellationToken? cancellationToken = null);
    }
}
