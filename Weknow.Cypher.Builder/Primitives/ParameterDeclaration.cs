#pragma warning disable CA1063 // Implement IDisposable Correctly

using System;

namespace Weknow.Cypher.Builder.Declarations
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
    public class ParameterDeclaration
    {
        private protected ParameterDeclaration() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        internal static readonly ParameterDeclaration Default = new ParameterDeclaration();

        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ParameterDeclaration operator +(ParameterDeclaration prm) => throw new NotImplementedException();

        /// <summary>
        /// Use mapping technique which don't use '{}' for the parameter.
        /// For example: CREATE (n $map)
        /// </summary>
        public IMap AsMap { get; }
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
    public class ParameterDeclaration<T> : ParameterDeclaration
    {
        private ParameterDeclaration() { }

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        new internal static readonly ParameterDeclaration<T> Default = new ParameterDeclaration<T>();
        
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
        public static implicit operator T(ParameterDeclaration<T> instance) => throw new NotImplementedException();
    }
}
