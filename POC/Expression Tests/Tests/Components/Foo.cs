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

    public class Foo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PropA { get; set; }
        public string PropB { get; set; }
    }

}
