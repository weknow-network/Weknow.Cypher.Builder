﻿using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    static class Schema
    {
        public static ILabel Person => throw new NotImplementedException();
        public static ILabel Tag => throw new NotImplementedException();
        public static ILabel Animal => throw new NotImplementedException();
        public static ILabel Maintainer => throw new NotImplementedException();
        public static IType KNOWS => throw new NotImplementedException();
        public static IType LIKE => throw new NotImplementedException();
        public static IType Affinity => throw new NotImplementedException();
        public static IType By => throw new NotImplementedException();
        public static IProperty Id => throw new NotImplementedException();
        public static IProperty Date => throw new NotImplementedException();
        public static IProperty PropA => throw new NotImplementedException();
        public static IProperty PropB => throw new NotImplementedException();
        public static IProperty PropC => throw new NotImplementedException();
        // public static IETagProperty Concurrency => throw new NotImplementedException();
    }

}
