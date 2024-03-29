﻿using Microsoft.Extensions.Logging;

using Neo4j.Driver;

using Weknow.CypherBuilder;
using Weknow.GraphDbClient.Abstraction;


namespace Weknow.GraphDbClient.Neo4jProvider;

/// <summary>
/// Neo4j graphDb abstraction
/// </summary>
internal sealed partial class N4jGraphDB : IGraphDB
{
    private readonly N4jSession _session;
    private readonly ILogger<N4jGraphDB> _logger;
    private readonly IAsyncQueryRunner _runner;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="N4jGraphDB" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    /// <param name="logger">The logger.</param>
    public N4jGraphDB(N4jSession session, ILogger<N4jGraphDB> logger)
    {
        _session = session;
        _logger = logger;
        _runner = session.Session;
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
        CypherParameters prms = parameters ?? cypherCommand.Parameters;
        var query = prms.Embed(cypherCommand);
        IResultCursor cursor = await _runner.RunAsync(query, prms);
        return await GraphDBResponse.Create(cursor, _logger);
    }
    #endregion // RunAsync

    #region StartTransaction



    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <returns></returns>
    Task<IGraphDBTransaction> IGraphDB.StartTransaction(TimeSpan? timeout)
    {
        var cfg = new GraphDBTransactionConfig { Timeout = timeout };
        return (this as IGraphDB).StartTransaction(cfg);
    }


    /// <summary>
    /// Starts a transaction.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    async Task<IGraphDBTransaction> IGraphDB.StartTransaction(Abstraction.GraphDBTransactionConfig configuration)
    {
        var tx = await _session.Session.BeginTransactionAsync(c =>
        {
            TimeSpan? timeout = configuration?.Timeout;
            if (timeout != null)
            {
                c.WithTimeout(timeout);
            }
        });

        return new N4jGraphDBTx(tx, _logger);
    }

    #endregion // StartTransaction
}

