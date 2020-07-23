using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Variable primitive for the Cypher expression with self formatting.
    /// </summary>
    /// <example>
    /// The n in the following expression will use the variable primitive. 
    /// SET n.Date = timestamp()
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface ISelfFormat : IPropertyOfType { }

}
