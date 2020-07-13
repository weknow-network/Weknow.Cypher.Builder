#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Label primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The Person in the following expression will use the Label primitive.
    /// MATCH (n:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface ILabel 
    {
        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n:Person:Animal)]]></example>
        public static ILabel operator & (ILabel a, ILabel b) => throw new NotImplementedException();
    }

}
