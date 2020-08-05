#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Properties primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The name, age in the following expression will use the Properties primitive.
    /// MATCH (n {name: 'Alice', age: 37})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    [Obsolete("No Magic", false)]

    public interface IPropertiesOfType: IProperties { }

}
