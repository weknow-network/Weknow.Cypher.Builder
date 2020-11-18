using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder.Declarations
{
    /// <summary>
    /// Variable primitive for the Cypher expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    /// The n in the following expression will use the variable primitive. 
    /// CREATE (n {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public class VariableDeclaration<T> : VariableDeclaration
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private VariableDeclaration() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        new internal static readonly VariableDeclaration<T> Default = new VariableDeclaration<T>();

        /// <summary>
        /// Gets type representation of the variable.
        /// </summary>
        public T _ { get; } 
        /// <summary>
        /// Gets type representation of the variable which should be increment.
        /// </summary>
        public T Inc { get; }

        /// <summary>
        /// Use the parameter as prefix
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static VariableDeclaration<T> operator ~(VariableDeclaration<T> instance) => throw new NotImplementedException();
    }

    /// <summary>
    /// Variable primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The n in the following expression will use the variable primitive. 
    /// CREATE (n {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public class VariableDeclaration
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private protected VariableDeclaration() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Default (and only) way to get cypher parameter.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        internal static readonly VariableDeclaration Default = new VariableDeclaration();


        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static VariableDeclaration operator +(VariableDeclaration l, VariableDeclaration r) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for unary operator +.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static VariableDeclaration operator +(VariableDeclaration item) => throw new NotImplementedException();

        /// <summary>
        /// Cast to parameter type.
        /// </summary>
        public ParameterDeclaration AsParameter { get; }
    }

}
