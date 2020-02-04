using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Pattern;
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
