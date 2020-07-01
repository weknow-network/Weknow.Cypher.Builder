using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace Weknow.Cypher.Builder.IntegrationTests
{
    static class Schema
    {
        public static ILabel Industry => throw new NotImplementedException();
        public static IType Affinity => throw new NotImplementedException();
    }

    static class SchemaProperties
    {
        public static IProperty Id => throw new NotImplementedException();
        public static IProperty Shape => throw new NotImplementedException();
        public static IProperty Keywords => throw new NotImplementedException();
        public static IProperty Color => throw new NotImplementedException();
        public static IProperty Label => throw new NotImplementedException();
        public static IProperty PriceFactor => throw new NotImplementedException();
        public static IProperty LayoutSize => throw new NotImplementedException();
        public static IProperty LayoutX => throw new NotImplementedException();
        public static IProperty LayoutY => throw new NotImplementedException();
    }
}
