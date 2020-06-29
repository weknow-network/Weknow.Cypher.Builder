using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Mapping variable primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// Used for queries like: 
    /// CREATE (n $map)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IMap
    {
        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IMap operator +(IMap map) => throw new NotImplementedException();
    }

}
