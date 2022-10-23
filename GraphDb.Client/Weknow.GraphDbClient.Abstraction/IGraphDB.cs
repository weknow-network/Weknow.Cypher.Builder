using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Weknow.Cypher.Builder;


namespace Weknow.GraphDbClient.Abstraction;

/// <summary>
/// GraphDB abstraction
/// </summary>
public interface IGraphDB
{
    /// <summary>
    /// Executes Cypher
    /// </summary>
    /// <param name="cypherCommand">The cypher command.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Response factory</returns>
    ValueTask<IGraphDBResponse> RunAsync(CypherCommand cypherCommand, CypherParameters? parameters = null);
}
