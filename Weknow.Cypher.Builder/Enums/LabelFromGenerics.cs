using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Indicate whether to use the generics argument as label
    /// </summary>
    [Obsolete("Deprecate", false)]
    public enum LabelFromGenerics 
    {
        /// <summary>
        /// The use
        /// </summary>
        Use,
        /// <summary>
        /// The ignore
        /// </summary>
        Ignore
    }
}
