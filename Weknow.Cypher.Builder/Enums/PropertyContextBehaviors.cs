using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Behaviors of Ignore Context.
    /// </summary>
    public enum PropertyContextBehaviors
    {
        /// <summary>
        /// Avoid this setting
        /// </summary>
        None,
        /// <summary>
        /// No formatting
        /// </summary>
        Plan,
        /// <summary>
        /// No formatting except the $ sign.
        /// </summary>
        Dolar
    }
}
