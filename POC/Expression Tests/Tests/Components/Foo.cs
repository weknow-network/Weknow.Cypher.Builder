using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    public class Foo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PropA { get; set; } = string.Empty;
        public string PropB { get; set; } = string.Empty;
    }

}
