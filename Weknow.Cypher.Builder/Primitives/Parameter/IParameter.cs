#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Parameter primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The $value in the following expression will use the Parameter primitive.
    /// CREATE (n:Person {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IParameter { }

    /// <summary>
    /// Parameter primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The $value in the following expression will use the Parameter primitive.
    /// CREATE (n:Person {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IParameter<T> : IParameter
    {
        /// <summary>
        /// Gets type representation of the variable.
        /// </summary>
        T _ { get; }
    }
}
