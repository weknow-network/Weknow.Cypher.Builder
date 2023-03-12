using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Function Extensions
/// </summary>
public partial class CypherExtensions
{
    #region Last / last(n)

    /// <summary>
    /// The function last() returns the last element in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN last(collect(n))
    /// </example>
    [Cypher("last($0)")]
    public static VariableDeclaration Last(this VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// The function last() returns the last element in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN last(collect(n))
    /// </example>
    [Cypher("last($0)")]
    public static VariableDeclaration<T> Last<T>(this VariableDeclaration<T> variable) => throw new NotImplementedException();

    #endregion // Last / last(n)

    #region Head / head(n)

    /// <summary>
    /// The function head() returns the first element in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN head(collect(n))
    /// </example>
    [Cypher("head($0)")]
    public static VariableDeclaration Head(this VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// The function head() returns the first element in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN head(collect(n))
    /// </example>
    [Cypher("head($0)")]
    public static VariableDeclaration<T> Head<T>(this VariableDeclaration<T> variable) => throw new NotImplementedException();

    #endregion // Head / head(n)

    #region Tail / tail(n)

    /// <summary>
    /// tail() returns a list lresult containing all the elements, excluding the first one, from a list list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN tail(collect(n))
    /// </example>
    [Cypher("tail($0)")]
    public static VariableDeclaration Tail(this VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// The function tail() returns the number of elements in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN tail(collect(n))
    /// </example>
    [Cypher("tail($0)")]
    public static VariableDeclaration<T> Tail<T>(this VariableDeclaration<T> variable) => throw new NotImplementedException();

    #endregion // Tail / tail(n)

    #region Size / size(n)

    /// <summary>
    /// The function size() returns the number of elements in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN size(collect(n))
    /// </example>
    [Cypher("size($0)")]
    public static VariableDeclaration Size(this VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// The function size() returns the number of elements in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN size(collect(n))
    /// </example>
    [Cypher("size($0)")]
    public static VariableDeclaration<T> Size<T>(this VariableDeclaration<T> variable) => throw new NotImplementedException();

    #endregion // Size / size(n)

    #region Reverse / reverse(n)

    /// <summary>
    /// reverse() returns a list in which the order of all elements in the original list have been reversed.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN reverse(collect(n))
    /// </example>
    [Cypher("reverse($0)")]
    public static VariableDeclaration Reverse(this VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// The function reverse() returns the number of elements in a list.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// MATCH (n)
    /// RETURN reverse(collect(n))
    /// </example>
    [Cypher("reverse($0)")]
    public static VariableDeclaration<T> Reverse<T>(this VariableDeclaration<T> variable) => throw new NotImplementedException();

    #endregion // Reverse / reverse(n)

    #region ToList

    /// <summary>
    /// Convert into a list with projection from the nodes in a path
    /// <![CDATA[[x IN nodes(path) | x.prop]]]>
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <remarks>
    /// https://neo4j.com/docs/cypher-manual/current/syntax/lists/#cypher-pattern-comprehension
    /// https://neo4j.com/docs/cypher-cheat-sheet/current/#_path_functions
    /// </remarks>
    /// <example><![CDATA[
    /// [x IN nodes(path) | x.prop]
    /// ]]></example>
    //[Cypher("$0\r\n[$2 IN $1 | $3]")]
    [CypherClause]
    public static VariableDeclaration ToList<T>(
        this VariableDeclaration<T> items,
        FluentToArrayAction<T> iteration) => throw new NotImplementedException();

    /// <summary>
    /// Convert into a list with projection from the nodes in a path
    /// <![CDATA[[x IN nodes(path) | x.prop]]]>
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="condition">Filter.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <remarks>
    /// https://neo4j.com/docs/cypher-manual/current/syntax/lists/#cypher-pattern-comprehension
    /// https://neo4j.com/docs/cypher-cheat-sheet/current/#_path_functions
    /// </remarks>
    /// <example><![CDATA[
    /// [x IN nodes(path) | x.prop]
    /// ]]></example>
    //[Cypher("$0\r\n[$2 IN $1 | $3]")]
    [CypherClause]
    public static VariableDeclaration ToList<T>(
        this VariableDeclaration<T> items,
        Predicate<VariableDeclaration<T>> condition,
        FluentToArrayAction<T> iteration) => throw new NotImplementedException();

    #endregion // ToList
}
