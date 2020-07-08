using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Property Options
    /// </summary>
    [Flags]
    public enum PropertyOptions
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// Detached from default formatting like UNWIND $items AS item
        /// Which will set the property by default to
        /// { Name: item.Name }
        /// </summary>
        Detached = 1
    }
}
