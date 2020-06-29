using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CypherAttribute : Attribute
    {
        public string Format { get; }

        public CypherAttribute(string format)
        {
            Format = format;
        }
    }

}
