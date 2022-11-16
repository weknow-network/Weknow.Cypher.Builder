using Weknow.Mapping;

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Cypher Parameters representation
    /// </summary>
    public class CypherParameters : IEnumerable<KeyValuePair<string, object?>>
    {
        private readonly Dictionary<string, object?> _parameters = new Dictionary<string, object?>();

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherParameters"/> class.
        /// </summary>
        public CypherParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherParameters"/> class.
        /// </summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        public CypherParameters(IDictionary<string, object?> dictionary)
        {
            _parameters = new Dictionary<string, object?>(dictionary);
        }

        #endregion // Ctor

        #region  Casting Overloads

        public static implicit operator Dictionary<string, object?>(CypherParameters parameters)
        {
            return parameters._parameters;
        }

        #endregion // Casting Overloads

        #region Add

        /// <summary>
        /// Adds IDictionaryable parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public new CypherParameters Add<T>(string key, T value) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            if (value is IDictionaryable da)
                _parameters[key] = da.ToDictionary();
            //else if (value is ValueType vt)
            //    _parameters[key] = vt;
            else
                _parameters[key] = value;
            return this;
        }

        #endregion // Add

        #region AddRange

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRange<T>(string key, params T[] values) // where T : IDictionaryable
        {
            return AddRange(key, (IEnumerable<T>)values);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRange<T>(string key, IEnumerable<T> values) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);
            _parameters[key] = values.Select(m =>
            {
                var result = m switch
                {
                    IDictionaryable da => (object)da.ToDictionary(),
                    _ => m
                };
                return result;
            });
            return this;
        }

        #endregion // AddRange

        #region AddNull

        /// <summary>
        /// Adds a null.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CypherParameters AddNull(string key)
        {
            _parameters[key] = null;
            return this;
        }

        #endregion // AddNull

        #region ContainsKey

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key) => _parameters.ContainsKey(key);

        #endregion // ContainsKey

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _parameters.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion // IEnumerable Members

        #region Get

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key) => (T)this[key];

        #endregion // Get

        #region object? this[string key]

        public object? this[string key]
        {
            get
            {
                if (_parameters.ContainsKey(key))
                    return _parameters[key];
                if (key.StartsWith("$"))
                    return _parameters[key.Substring(1)];
                throw new KeyNotFoundException(key);
            }
            set
            {
                if (key.StartsWith("$"))
                    key = key.Substring(1);
                if (value is IDictionaryable d)
                    _parameters[key] = d.ToDictionary();
                else if (value is IEnumerable<IDictionaryable> ds)
                    _parameters[key] = ds.Select(m => m.ToDictionary());
                else
                    _parameters[key] = value;
            }
        }

        #endregion // object? this[string key]

        #region Count

        public int Count => _parameters.Count;

        #endregion // Count
    }

}
