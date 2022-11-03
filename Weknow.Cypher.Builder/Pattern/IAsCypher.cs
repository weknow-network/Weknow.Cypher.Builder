using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    public interface IAsCypher : IPattern
    {       
    }

}
