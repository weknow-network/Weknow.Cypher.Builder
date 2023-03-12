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
}
