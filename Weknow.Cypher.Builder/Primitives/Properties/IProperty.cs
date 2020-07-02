#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Property primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The name in the following expression will use the Property primitive.
    /// MATCH (n {name: 'Alice'})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IProperty { }
}
