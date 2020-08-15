#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Represent SET's clause expression.
    /// </summary>
    /// <example>
    /// The KNOWS in the following expression will use the Type primitive.
    /// MATCH (n:Person)-[:KNOWS]->(m:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface ISetExpression
    {
    }
}
