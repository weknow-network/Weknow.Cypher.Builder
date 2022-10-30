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


        CypherParameters AddValue<T>(string key, T value) where T : struct
        {
            this[key] = value;
            return this;
        }

        CypherParameters Add<T>(string key, T value) where T: IDictionaryable
        {
            this[key] = value.ToDictionary();
            return this;
        }

        CypherParameters AddValueRange<T>(string key, params T[] values) where T : struct
        {
            this[key] = values;
            return this;
        }

        CypherParameters AddRange<T>(string key, params T[] value) where T: IDictionaryable
        {
            this[key] = value.Select(m => m.ToDictionary()).ToArray();
            return this;
        }
    }

}
