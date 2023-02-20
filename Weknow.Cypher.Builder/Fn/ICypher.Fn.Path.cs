using Weknow.CypherBuilder.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/
// https://neo4j.com/docs/cypher-cheat-sheet/current/#_path_functions

namespace Weknow.CypherBuilder;

partial interface ICypher
{
    partial interface IFn
    {
        /// <summary>
        /// Path's functions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICypherPath Path => throw new NotImplementedException();
    }

    public interface ICypherPath
    {
        #region Length / length(n)

        /// <summary>
        /// List from the values, ignores null.
        /// </summary>
        /// <param name="result">The result to be length.</param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// MATCH p = (a)-->(b)-->(c)
        /// WHERE a.name = 'Alice'
        /// RETURN length(p)
        /// ]]>
        /// </example>
        [Cypher("length($0)")]
        public VariableDeclaration Length(object result) => throw new NotImplementedException();

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
        public NodesVariableDeclaration Nodes(PathVariableDeclaration path) => throw new NotImplementedException();

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
        public NodesVariableDeclaration<T> Nodes<T>(PathVariableDeclaration<T> path) => throw new NotImplementedException();

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
        public RelationsVariableDeclaration Relationships(PathVariableDeclaration path) => throw new NotImplementedException();

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
        public RelationsVariableDeclaration<T> Relationships<T>(PathVariableDeclaration<T> path) => throw new NotImplementedException();

        #endregion // Relationships / relationships(n)
    }
}
