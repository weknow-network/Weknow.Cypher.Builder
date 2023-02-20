using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Function Extensions
/// </summary>
public partial class CypherExtensions
{
    #region Length / length(n)

    /// <summary>
    /// Length of a path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// MATCH p = (a)-->(b)-->(c)
    /// WHERE a.name = 'Alice'
    /// RETURN length(p)
    /// ]]>
    /// </example>
    [Cypher("length($0)")]
    public static VariableDeclaration Length(this VariableDeclaration path) => throw new NotImplementedException();

    #endregion // Length / length(n)

    #region Nodes / nodes(n)

    /// <summary>
    /// Return the nodes in the path as a list.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// MATCH p = (a)-->(b)-->(c)
    /// RETURN nodes(p)
    /// ]]>
    /// </example>
    [Cypher("nodes($0)")]
    public static NodesVariableDeclaration Nodes(this PathVariableDeclaration path) => throw new NotImplementedException();

    /// <summary>
    /// Return the nodes in the path as a list.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// MATCH p = (a)-->(b)-->(c)
    /// RETURN nodes(p)
    /// ]]>
    /// </example>
    [Cypher("nodes($0)")]
    public static NodesVariableDeclaration<T> Nodes<T>(this PathVariableDeclaration<T> path) => throw new NotImplementedException();

    #endregion // Nodes / nodes(n)

    #region Relationships / relationships(n)

    /// <summary>
    /// Return the relationships in the path as a list.
    /// </summary>
    /// <param name="path">The rpath.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// MATCH p = (a)-->(b)-->(c)
    /// RETURN relationships(p)
    /// ]]>
    /// </example>
    [Cypher("relationships($0)")]
    public static RelationsVariableDeclaration Relationships(this PathVariableDeclaration path) => throw new NotImplementedException();

    /// <summary>
    /// Return the relationships in the path as a list.
    /// </summary>
    /// <param name="path">The rpath.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[
    /// MATCH p = (a)-->(b)-->(c)
    /// RETURN relationships(p)
    /// ]]>
    /// </example>
    [Cypher("relationships($0)")]
    public static RelationsVariableDeclaration<T> Relationships<T>(this PathVariableDeclaration<T> path) => throw new NotImplementedException();

    #endregion // Relationships / relationships(n)

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
        this PathRelatedVariableDeclaration<T> items,
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
        this PathRelatedVariableDeclaration<T> items,
        Predicate<VariableDeclaration<T>> condition,
        FluentToArrayAction<T> iteration) => throw new NotImplementedException();

    #endregion // ToList
}
