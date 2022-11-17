using Neo4j.Driver;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal partial class N4jGraphDB : IGraphDB
{
    private readonly N4jSession _session;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    public N4jGraphDB(N4jSession session)
    {
        _session = session;
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
    async ValueTask<IGraphDBResponse> IGraphDB.RunAsync(CypherCommand cypherCommand, CypherParameters? parameters)
    {
        IResultCursor cursor = await _session.Session.RunAsync(cypherCommand, parameters ?? cypherCommand.Parameters);
        return await GraphDBResponse.Create(cursor);
    }

    #endregion // RunAsync
}
