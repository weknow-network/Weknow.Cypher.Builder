using System.Collections.Generic;
using System.Runtime.Serialization;

using Weknow.Mapping;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Cypher Parameters representation
    /// </summary>
    public class CypherParameters : Dictionary<string, object?>
    {
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
        public CypherParameters(IDictionary<string, object?> dictionary) : base(dictionary)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CypherParameters"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public CypherParameters(int capacity) : base(capacity)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CypherParameters"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected CypherParameters(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion // Ctor

        #region // Casting Overloads

        //public static implicit operator Dictionary<string, object?>(CypherParameters parameters)
        //{
        //    return parameters;
        //}

        #endregion // Casting Overloads

        #region AddString

        /// <summary>
        /// Add string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public CypherParameters AddString(string key, string value) 
        {
            this[key] = value;
            return this;
        }

        #endregion // AddString

        #region AddValue

        /// <summary>
        /// Add simple value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public CypherParameters AddValue<T>(string key, T value) where T : struct
        {
            this[key] = value;
            return this;
        }

        #endregion // AddValue

        #region Add

        /// <summary>
        /// Adds IDictionaryable parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public new CypherParameters Add<T>(string key, T value) where T: IDictionaryable
        {
            this[key] = value.ToDictionary();
            return this;
        }

        #endregion // Add

        #region AddStringRange

        /// <summary>
        /// Adds parameter of a range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddStringRange(string key, params string[] values)
        {
            return AddStringRange(key, (IEnumerable<string>)values);
        }

        /// <summary>
        /// Adds parameter of a range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddStringRange(string key, IEnumerable<string> values) 
        {
            this[key] = values;
            return this;
        }

        #endregion // AddStringRange

        #region AddValueRange

        /// <summary>
        /// Adds parameter of a range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddValueRange<T>(string key, params T[] values) where T : struct
        {
            return AddValueRange(key, (IEnumerable<T>)values);
        }


        /// <summary>
        /// Adds parameter of a range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddValueRange<T>(string key, IEnumerable<T> values) where T : struct
        {
            this[key] = values;
            return this;
        }

        #endregion // AddValueRange

        #region AddRange

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public CypherParameters AddRange<T>(string key, params T[] values) where T: IDictionaryable
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
        public CypherParameters AddRange<T>(string key, IEnumerable<T> values) where T: IDictionaryable
        {
            this[key] = values.Select(m => m.ToDictionary()).ToArray();
            return this;
        }

        #endregion // AddRange
    }

}
