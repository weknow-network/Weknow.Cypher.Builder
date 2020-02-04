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
    public class DisposeableAction : IDisposable
    {
        Action _action;

        public DisposeableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }

}
