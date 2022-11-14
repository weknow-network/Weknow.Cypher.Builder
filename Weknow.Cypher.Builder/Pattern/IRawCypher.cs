using System;
using System.Reflection;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    public interface IRawCypher : IPattern
    {       
    }
}
