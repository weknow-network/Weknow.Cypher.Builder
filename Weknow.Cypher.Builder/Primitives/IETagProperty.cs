#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// ETag Property is a special primitive which will translate 
    /// to optimistic concurrency pattern in the Cypher expression.
    /// </summary>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IETagProperty { }

}
