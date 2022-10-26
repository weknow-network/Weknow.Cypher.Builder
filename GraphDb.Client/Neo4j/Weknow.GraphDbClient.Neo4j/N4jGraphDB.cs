﻿using Neo4j.Driver;

using Weknow.GraphDbCommands;
using Weknow.GraphDbClient.Abstraction;
using System.Reflection;
using EnsureThat;

namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal partial class N4jGraphDB : IGraphDB
{
    private readonly IAsyncSession _session;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    public N4jGraphDB(IAsyncSession session)
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
        IResultCursor cursor = await _session.RunAsync(cypherCommand, parameters);
        return new GraphDBResponse(cursor);
    }

    #endregion // RunAsync
}
