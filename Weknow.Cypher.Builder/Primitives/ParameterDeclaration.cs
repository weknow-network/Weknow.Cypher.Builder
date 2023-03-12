using Weknow.Cypher.Builder.Fluent;

namespace Weknow.CypherBuilder.Declarations;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

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
public class ParameterDeclaration : ICypherToken
{
    private protected ParameterDeclaration() { }

    /// <summary>
    /// Default (and only) way to get cypher parameter.
    /// It use under expression and don't need a real implementation;
    /// </summary>
    internal static readonly ParameterDeclaration Default = new ParameterDeclaration();


    /// <summary>
    /// Gets type representation of the parameter. 
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// Merge(N(n, Person, new { m._<Foo>.Id })))
    /// will result in
    /// MERGE (n:Person {{ Id: $Id }})
    /// ]]>
    /// </example>
    public T _<T>() => throw new NotImplementedException();


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
    public static bool operator ==(object? a, ParameterDeclaration? b) => throw new NotImplementedException();

    /// <summary>
    /// Declaration for operator !=.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(object? a, ParameterDeclaration? b) => throw new NotImplementedException();

    ///// <summary>
    ///// Declaration for operator ==.
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns>
    ///// The result of the operator.
    ///// </returns>
    //public static bool operator ==(ParameterDeclaration? a, object? b) => throw new NotImplementedException();

    ///// <summary>
    ///// Declaration for operator !=.
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns>
    ///// The result of the operator.
    ///// </returns>
    //public static bool operator !=(ParameterDeclaration? a, object? b) => throw new NotImplementedException();

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
    public static bool operator >(ParameterDeclaration a, object b) => throw new NotImplementedException();

    /// <summary>
    /// <![CDATA[Declaration for operator <.]]>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(ParameterDeclaration a, object b) => throw new NotImplementedException();

    /// <summary>
    /// <![CDATA[Declaration for operator >.]]>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(object a, ParameterDeclaration b) => throw new NotImplementedException();

    /// <summary>
    /// <![CDATA[Declaration for operator <.]]>
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
    /// Gets type representation of the variable. while preserving the path to the parameter
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// Merge(N(n, Person, new { map.__.Id }))
    /// will result in
    /// MERGE (n:Person {{ Id: $map.Id }})
    /// ]]>
    /// </example>
    public T __ { get; }

    /// <summary>
    /// Gets type representation of the variable. without preserving the path to the parameter
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// Merge(N(n, Person, new { map._.Id }))
    /// will result in
    /// MERGE (n:Person {{ Id: $Id }})
    /// ]]>
    /// </example>
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
    [Obsolete("Use .__ instead")]
    public static ParameterDeclaration<T> operator ~(ParameterDeclaration<T> instance) => throw new NotImplementedException();
}
