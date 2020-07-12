#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Relation's Type primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The KNOWS in the following expression will use the Type primitive.
    /// MATCH (n:Person)-[:KNOWS]->(m:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public sealed class CypherType 
    {
        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n)-[:KNOWS|:LOVES]->(m)]]></example>
        public static CypherType operator |(CypherType a, CypherType b) => throw new NotImplementedException();
    }

}
