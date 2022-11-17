using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Cypher Function Extensions
    /// </summary>
    public partial class CypherExtensions
    {
        #region Type / type(r)

        /// <summary>
        /// String representation of the relationship type.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)-[r:LOVE]->(m)
        /// RETURN type(r)
        /// </example>
        [Cypher("type($0)")]
        public static VariableDeclaration Type(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // Type / type(r)

        #region StartNode / startNode(r)

        /// <summary>
        /// Start node of the relationship.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)-[r:LOVE]->(m)
        /// RETURN startNode(r)
        /// </example>
        [Cypher("startNode($0)")]
        public static VariableDeclaration StartNode(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // StartNode / startNode(r)

        #region EndNode / endNode(r)

        /// <summary>
        /// End node of the relationship.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)-[r:LOVE]->(m)
        /// RETURN endNode(r)
        /// </example>
        [Cypher("endNode($0)")]
        public static VariableDeclaration EndNode(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // EndNode / endNode(r)

        #region Id / id(n)

        /// <summary>
        /// The internal id of the relationship.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN id(n)
        /// </example>
        [Cypher("id($0)")]
        public static VariableDeclaration Id(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // Id / id(n)

        #region type / type(n)

        /// <summary>
        /// Labels of the node.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n:PERSON:ADMIN)
        /// RETURN labels(n)
        /// </example>
        [Cypher("type($0)")]
        public static VariableDeclaration type(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // type / type(n)

        #region Labels / labels(n)

        /// <summary>
        /// Labels of the node.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n:PERSON:ADMIN)
        /// RETURN labels(n)
        /// </example>
        [Cypher("labels($0)")]
        public static VariableDeclaration Labels(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // Labels / labels(n)

        #region Count / count(n)

        /// <summary>
        /// Count the results.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN count(n)
        /// </example>
        [Cypher("count($0)")]
        public static VariableDeclaration Count(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // Count / count(n)

        #region CountDistinct / count(DISTINCT n)

        /// <summary>
        /// Count the results.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN count(n)
        /// </example>
        [Cypher("count(DISTINCT $0)")]
        public static VariableDeclaration CountDistinct(this VariableDeclaration variable) => throw new NotImplementedException();

        #endregion // CountDistinct / count(DISTINCT n)

        #region Collect / collect(n), collect(n.PropA)

        /// <summary>
        /// List from the values, ignores null.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// n.Collect() or Collect(n)
        /// result in:
        /// collect(n)
        /// </example>
        [Cypher("collect($0)")]
        public static VariableDeclaration Collect(this VariableDeclaration variable) => throw new NotImplementedException();

        /// <summary>
        /// List from the values, ignores null.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        /// <example>
        /// Collect(n._.Id)
        /// result in
        /// collect(n.Id)
        /// </example>
        [Cypher("collect($0)")]
        public static VariableDeclaration Collect(object property) => throw new NotImplementedException();

        #endregion // Collect / collect(n), collect(n.PropA)

        #region CollectDistinct / collect(DISTINCT n), collect(DISTINCT n.PropA)

        /// <summary>
        /// List from the values, ignores null.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN collect(DISTINCT n)
        /// </example>
        [Cypher("collect(DISTINCT $0)")]
        public static VariableDeclaration CollectDistinct(this VariableDeclaration variable) => throw new NotImplementedException();

        /// <summary>
        /// List from the values, ignores null.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN collect(DISTINCT n.PropA)
        /// </example>
        [Cypher("collect(DISTINCT $0)")]
        public static VariableDeclaration CollectDistinct(object prop) => throw new NotImplementedException();

        #endregion // CollectDistinct / collect(DISTINCT n), collect(DISTINCT n.PropA)

        #region // Coalesce / coalesce(n)

        ///// <summary>
        ///// Count the results.
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="prop">The property.</param>
        ///// <param name="defaultValue">The default value.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MATCH (n)
        ///// RETURN coalesce(n.Prop, 'x')
        ///// </example>
        //[Cypher("coalesce($0)")]
        //public static VariableDeclaration Coalesce(
        //                    this VariableDeclaration variable,
        //                    IParameter prop,
        //                    object defaultValue) => throw new NotImplementedException();

        ///// <summary>
        ///// Count the results.
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="defaultValue">The default value.</param>
        ///// <returns></returns>
        ///// <exception cref="System.NotImplementedException"></exception>
        ///// <example>
        ///// MATCH (n)
        ///// RETURN coalesce(n.Prop, 'x')
        ///// </example>
        //[Cypher("coalesce($0, $1)")]
        //public static VariableDeclaration Coalesce(
        //                    this VariableDeclaration variable, 
        //                    IPattern defaultValue) => throw new NotImplementedException();
        ///// <summary>
        ///// Count the results.
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="prop">The property.</param>
        ///// <param name="defaultValue">The default value.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MATCH (n)
        ///// RETURN coalesce(n.Prop, 'x')
        ///// </example>
        //[Cypher("coalesce($0)")]
        //public static VariableDeclaration Coalesce(
        //                    this VariableDeclaration variable,
        //                    IPattern defaultValue) => throw new NotImplementedException();

        #endregion // Coalesce / coalesce(n)
    }
}
