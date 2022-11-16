namespace Weknow.GraphDbCommands.Declarations
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

        #region +

        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="prm">The PRM.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static ParameterDeclaration operator +(ParameterDeclaration prm) => throw new NotImplementedException();

        #endregion // +


        #region == / !=

        /// <summary>
        /// Declaration for operator ==.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(object a, ParameterDeclaration b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator !=.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(object a, ParameterDeclaration b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator ==.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ParameterDeclaration a, object b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator !=.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ParameterDeclaration a, object b) => throw new NotImplementedException();

        #endregion // == / !=

        #region <>

        /// <summary>
        /// Declaration for operator >.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(ParameterDeclaration a, object b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator <.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(ParameterDeclaration a, object b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator >.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(object a, ParameterDeclaration b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator <.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(object a, ParameterDeclaration b) => throw new NotImplementedException();

        #endregion // <>
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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ParameterDeclaration() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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

        /// <summary>
        /// Use the parameter as prefix
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static ParameterDeclaration<T> operator ~(ParameterDeclaration<T> instance) => throw new NotImplementedException();
    }
}
