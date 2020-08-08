#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Parameter primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The $value in the following expression will use the Parameter primitive.
    /// CREATE (n:Person {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public class Parameter 
    {
        private protected Parameter() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        public static readonly Parameter Default = new Parameter();

        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Parameter operator +(Parameter prm) => throw new NotImplementedException();
    }

    /// <summary>
    /// Parameter primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The $value in the following expression will use the Parameter primitive.
    /// CREATE (n:Person {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public class Parameter<T> : Parameter
    {
        private Parameter() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        public static readonly Parameter<T> Default = new Parameter<T>();
        
        /// <summary>
        /// Gets type representation of the variable.
        /// </summary>
        public T _ { get; }


        /// <summary>
        /// Casting overload.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T(Parameter<T> instance) => throw new NotImplementedException();
    }
}
