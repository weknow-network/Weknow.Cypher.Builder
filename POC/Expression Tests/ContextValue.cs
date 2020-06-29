using System;
using System.Collections.Generic;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    public class ContextValue<T>
    {
        private Stack<T> values = new Stack<T>();

        public T Value
        {
            get => values.Peek();
        }

        public ContextValue(T defaultValue)
        {
            values.Push(defaultValue);
        }

        public IDisposable Set(T value)
        {
            values.Push(value);
            return new DisposeableAction(() => values.Pop());
        }

        public static implicit operator T(ContextValue<T> context) => context.Value;
    }

}
