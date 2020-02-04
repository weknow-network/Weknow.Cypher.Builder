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

    static class Schema
    {
        public static ILabel Person => throw new NotImplementedException();
        public static IType KNOWS => throw new NotImplementedException();
        public static IProperty Id => throw new NotImplementedException();
        public static IProperty PropA => throw new NotImplementedException();
        public static IProperty PropB => throw new NotImplementedException();
        public static IETagProperty Concurrency => throw new NotImplementedException();
    }

}
