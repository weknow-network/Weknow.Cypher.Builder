using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    static class Schema
    {
        public static CypherLabel Person => throw new NotImplementedException();
        public static CypherLabel Animal => throw new NotImplementedException();
        public static CypherLabel Maintainer => throw new NotImplementedException();
        public static CypherType KNOWS => throw new NotImplementedException();
        public static CypherType LIKE => throw new NotImplementedException();
        public static CypherType By => throw new NotImplementedException();
        public static IProperty Id => throw new NotImplementedException();
        public static IProperty Date => throw new NotImplementedException();
        public static IProperty PropA => throw new NotImplementedException();
        public static IProperty PropB => throw new NotImplementedException();
        public static IProperty PropC => throw new NotImplementedException();
        // public static IETagProperty Concurrency => throw new NotImplementedException();
    }

}
