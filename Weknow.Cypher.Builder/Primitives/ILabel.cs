#pragma warning disable CA1063 // Implement IDisposable Correctly

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
    public interface ILabel { }

}
