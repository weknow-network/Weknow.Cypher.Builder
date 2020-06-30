using System;
using System.Collections.Generic;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Context container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContextValue<T>
    {
        private Stack<T> values = new Stack<T>();

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextValue{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        public ContextValue(T defaultValue)
        {
            values.Push(defaultValue);
        }

        #endregion // Ctor

        #region Value

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value  => values.Peek();

        #endregion // Value

        #region Set

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IDisposable Set(T value)
        {
            values.Push(value);
            return new DisposeableAction(() => values.Pop());
        }

        #endregion // Set

        #region Casting Operators

        /// <summary>
        /// Performs an implicit conversion.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T(ContextValue<T> context) => context.Value;

        #endregion // Casting Operators
    }
}
