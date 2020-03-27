using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Parameters representation
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{System.String, System.Object}" />
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

        //public static implicit operator CypherParameters(IDictionary<string, object?> parameters)
        //{
        //    var result = new CypherParameters(parameters);
        //    return result;
        //}

        #endregion // Casting Overloads
    }

}
