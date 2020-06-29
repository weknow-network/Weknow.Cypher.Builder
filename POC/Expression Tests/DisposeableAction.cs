using System;
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
