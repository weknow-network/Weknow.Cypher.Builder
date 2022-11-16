
namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Context container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContextValue<T> : IEnumerable<T>
    {
        private ImmutableStack<T> _values = ImmutableStack<T>.Empty;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextValue{T}"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        public ContextValue(T defaultValue)
        {
            _values = _values.Push(defaultValue);
        }

        #endregion // Ctor

        #region Value

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value => _values.Peek();

        #endregion // Value

        #region Values

        /// <summary>
        /// Gets all values.
        /// </summary>
        public ImmutableStack<T> Values => _values;

        #endregion // Values

        #region Set

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IDisposable Set(T value)
        {
            _values = _values.Push(value);
            return new DisposeableAction(() => _values = _values.Pop());
        }

        #endregion // Set

        #region Set

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <returns></returns>
        public IDisposable Set(T value, bool condition)
        {
            if (!condition)
                return DisposeableAction.Empty;

            _values = _values.Push(value);
            return new DisposeableAction(() => _values = _values.Pop());
        }

        #endregion // Set

        #region IEnumerable<T> members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in _values)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion // IEnumerable<T> members

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
