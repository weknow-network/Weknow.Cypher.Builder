using Weknow.Mapping;

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Cypher Parameters representation
    /// </summary>
    public class CypherParameters : IEnumerable<KeyValuePair<string, object?>>
    {
        private ImmutableDictionary<string, object?> _parameters = ImmutableDictionary<string, object?>.Empty;

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
            _parameters = ImmutableDictionary.CreateRange(dictionary);
        }

        #endregion // Ctor

        #region  Casting Overloads

        public static implicit operator Dictionary<string, object?>(CypherParameters parameters)
        {
            IEnumerable<KeyValuePair<string, object?>> ps = parameters;
            var result = new Dictionary<string, object?>(ps);
            return result;
        }

        #endregion // Casting Overloads

        #region AddIfEmpty

        /// <summary>
        /// Adds parameter if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public CypherParameters AddIfEmpty<T>(string key, T value) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                return this;
            if (value is IDictionaryable da)
                _parameters = parameters.Add(key, da.ToDictionary());
            //else if (value is ValueType vt)
            //    _parameters[key] = vt;
            else
                _parameters = parameters.Add(key, value);
            return this;
        }

        #endregion // AddIfEmpty

        #region AddRangeIfEmpty

        /// <summary>
        /// Adds parameters if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRangeIfEmpty<T>(string key, params T[] values) // where T : IDictionaryable
        {
            return AddRangeIfEmpty(key, (IEnumerable<T>)values);
        }

        /// <summary>
        /// Adds parameters if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRangeIfEmpty<T>(string key, IEnumerable<T> values) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                return this;
            var range = values.Select(m =>
            {
                var result = m switch
                {
                    IDictionaryable da => (object)da.ToDictionary(),
                    _ => m
                };
                return result;
            });
            _parameters = parameters.Add(key, range);
            return this;
        }

        #endregion // AddRangeIfEmpty

        #region AddOrUpdate

        /// <summary>
        /// Adds parameter or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="overrideMapping">
        /// Optional update decision mapping.
        /// If not exists the value will simply override 
        /// </param>
        /// <returns></returns>
        public CypherParameters AddOrUpdate<T>(
            string key,
            T value,
            Func<T, object?>? overrideMapping = null) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                parameters = parameters.Remove(key);
            if (overrideMapping != null)
            { 
                var mapped = overrideMapping(value);
                if (mapped is IDictionaryable dam)
                    _parameters = parameters.Add(key, dam.ToDictionary());
                //else if (value is ValueType vt)
                //    _parameters[key] = vt;
                else
                    _parameters = parameters.Add(key, mapped);
            }
            if (value is IDictionaryable da)
                _parameters = parameters.Add(key, da.ToDictionary());
            //else if (value is ValueType vt)
            //    _parameters[key] = vt;
            else
                _parameters = parameters.Add(key, value);
            return this;
        }

        #endregion // AddOrUpdate

        #region AddRangeOrUpdate

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRangeOrUpdate<T>(
            string key, 
            params T[] values) // where T : IDictionaryable
        {
            return AddRangeOrUpdate(key, (IEnumerable<T>)values);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideMapping">
        /// Optional update decision mapping.
        /// If not exists the value will simply override 
        /// </param>
        /// <returns></returns>
        public CypherParameters AddRangeOrUpdate<T>(
            string key, 
            IEnumerable<T> values,
            Func<T, object?>? overrideMapping = null) // where T : IDictionaryable
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                parameters = parameters.Remove(key);
            var range = values.Select(m =>
            {
                object? value = m;
                if (overrideMapping != null)
                    value = overrideMapping(m); 
                var result = value switch
                {                    
                    IDictionaryable da => (object)da.ToDictionary(),
                    _ => value
                };
                return result;
            });
            _parameters = parameters.Add(key, range);
            return this;
        }

        #endregion // AddRangeOrUpdate

        #region SetToNull

        /// <summary>
        /// Adds a null.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CypherParameters SetToNull(string key)
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                parameters = parameters.Remove(key);

            _parameters = parameters.Add(key, null);
            return this;
        }

        #endregion // SetToNull

        #region TrySetToNull

        /// <summary>
        /// Adds a null.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CypherParameters TrySetToNull(string key)
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                return this;

            _parameters = parameters.Add(key, null);
            return this;
        }

        #endregion // TrySetToNull

        #region Remove

        /// <summary>
        /// Remove a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public CypherParameters Remove(string key) 
        {
            if (key.StartsWith("$"))
                key = key.Substring(1);

            var parameters = _parameters;
            if (parameters.ContainsKey(key))
                _parameters = parameters.Remove(key);
            return this;
        }

        #endregion // Remove

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
#pragma warning disable CS8600
#pragma warning disable CS8603
        public T Get<T>(string key) => (T)this[key];
#pragma warning restore CS8603 
#pragma warning restore CS8600

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
            //set
            //{
            //    if (key.StartsWith("$"))
            //        key = key.Substring(1);
            //    if (value is IDictionaryable d)
            //        _parameters[key] = d.ToDictionary();
            //    else if (value is IEnumerable<IDictionaryable> ds)
            //        _parameters[key] = ds.Select(m => m.ToDictionary());
            //    else
            //        _parameters[key] = value;
            //}
        }

        #endregion // object? this[string key]

        #region Count

        public int Count => _parameters.Count;

        #endregion // Count
    }

}
