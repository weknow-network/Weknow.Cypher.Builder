#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.  

using Weknow.Cypher.Builder.Fluent;

namespace Weknow.CypherBuilder.Declarations
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
        private VariableDeclaration() { }

        /// <summary>
        /// Default (and only) way to get cypher variable.
        /// It use under expression and don't need a real implementation;
        /// </summary>
        new internal static readonly VariableDeclaration<T> Default = new VariableDeclaration<T>();

        /// <summary>
        /// Gets type representation of the variable.
        /// </summary>
        public T _ { get; }

        /// <summary>
        /// Gets type representation of the variable. while preserving the path.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// Unwind(items, map,
        /// Merge(N(n, Person, new { map.__.Id })))
        /// will result in
        /// UNWIND $items AS map
        /// MERGE (n:Person {{ Id: map.Id }})
        /// ]]>
        /// </example>
        public T __ { get; }

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
        [Obsolete("deprecated: use x.__ instead of (~x)._", false)]
        public static VariableDeclaration<T> operator ~(VariableDeclaration<T> instance) => throw new NotImplementedException();


        /// <summary>
        /// Cast to parameter type.
        /// </summary>
        public new ParameterDeclaration<T> AsParameter { get; }
        /// <summary>
        /// Cast to parameter type.
        /// </summary>
        public new ParameterDeclaration<T> Prm { get; }

    }

    public class TimeVariableDeclaration: VariableDeclaration
    { 
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
    public class VariableDeclaration: ICypherToken
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
        /// Gets type representation of the variable. 
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// Unwind(items, map,
        /// Merge(N(n, Person, new { map._<Foo>.Id })))
        /// will result in
        /// UNWIND $items AS map
        /// MERGE (n:Person {{ Id: Id }})
        /// ]]>
        /// </example>
        public T _<T>() => throw new NotImplementedException();

        /// <summary>
        /// Gets type representation of the variable. while preserving the path.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// Unwind(items, map,
        /// Merge(N(n, Person, new { map.__<Foo>.Id })))
        /// will result in
        /// UNWIND $items AS map
        /// MERGE (n:Person {{ Id: map.Id }})
        /// ]]>
        /// </example>
        public T __<T>() => throw new NotImplementedException();

        #region == / !=

        /// <summary>
        /// Declaration for operator ==.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(VariableDeclaration a, object b) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for operator !=.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(VariableDeclaration a, object b) => throw new NotImplementedException();

        ///// <summary>
        ///// Declaration for operator ==.
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="b"></param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static bool operator ==(object a, VariableDeclaration b) => throw new NotImplementedException();

        ///// <summary>
        ///// Declaration for operator !=.
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="b"></param>
        ///// <returns>
        ///// The result of the operator.
        ///// </returns>
        //public static bool operator !=(object a, VariableDeclaration b) => throw new NotImplementedException();

        #endregion // == / !=

        #region <>

        /// <summary>
        /// <![CDATA[Declaration for operator >.]]>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(VariableDeclaration? a, object? b) => throw new NotImplementedException();

        /// <summary>
        /// <![CDATA[Declaration for operator <.]]>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(VariableDeclaration? a, object? b) => throw new NotImplementedException();

        /// <summary>
        /// <![CDATA[Declaration for operator >.]]>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(object? a, VariableDeclaration? b) => throw new NotImplementedException();

        /// <summary>
        /// <![CDATA[Declaration for operator <.]]>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(object? a, VariableDeclaration? b) => throw new NotImplementedException();

        #endregion // <>

        #region +

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

        #endregion // +

        /// <summary>
        /// Cast to parameter type.
        /// </summary>
        public ParameterDeclaration AsParameter { get; }
        /// <summary>
        /// Cast to parameter type.
        /// </summary>
        public ParameterDeclaration Prm { get; }

        /// <summary>
        /// Avoid ambient label attachment
        /// </summary>
        public VariableDeclaration NoAmbient { get; }  
    }

}
