﻿using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{

    static class Schema
    {
        public static ILabel Person => throw new NotImplementedException();
        public static ILabel Animal => throw new NotImplementedException();
        public static ILabel Manager => throw new NotImplementedException();
        public static ILabel Maintainer => throw new NotImplementedException();
        public static IType KNOWS => throw new NotImplementedException();
        public static IType LIKE => throw new NotImplementedException();
        public static IType By => throw new NotImplementedException();
    }
}
