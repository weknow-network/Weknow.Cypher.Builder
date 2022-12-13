using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

namespace Weknow.Cypher.Builder
{
    internal static class MockGenerator
    {
        private static readonly ProxyGenerator _gen = new ProxyGenerator();

        public static T MockInterface<T>() where T : class => _gen.CreateInterfaceProxyWithoutTarget<T>();
    }
}
