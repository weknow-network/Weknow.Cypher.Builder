using System;
using System.Text;

using Weknow.GraphDbCommands.Declarations;

using static Weknow.GraphDbCommands.CypherDelegates;

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Cypher Function Extensions
    /// </summary>
    partial class Cypher
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

        #region Sum / sum(n.PropA))

        /// <summary>
        /// Sum numerical values. Similar functions are avg(), min(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN sum(n.PropA)
        /// </example>
        [Cypher("sum($0)")]
        public static VariableDeclaration Sum(object prop) => throw new NotImplementedException();

        #endregion // Sum / sum(n.PropA))

        #region Max / max(n.PropA))

        /// <summary>
        /// Max numerical values. Similar functions are avg(), min(), sum().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN max(n.PropA)
        /// </example>
        [Cypher("max($0)")]
        public static VariableDeclaration Max(object prop) => throw new NotImplementedException();

        #endregion // Max / max(n.PropA))

        #region Min / min(n.PropA))

        /// <summary>
        /// Min numerical values. Similar functions are avg(), sum(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN min(n.PropA)
        /// </example>
        [Cypher("min($0)")]
        public static VariableDeclaration Min(object prop) => throw new NotImplementedException();

        #endregion // Min / min(n.PropA))

        #region Avg / avg(n.PropA))

        /// <summary>
        /// Avg numerical values. Similar functions are sum(), min(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN avg(n.PropA)
        /// </example>
        [Cypher("avg($0)")]
        public static VariableDeclaration Avg(object prop) => throw new NotImplementedException();

        #endregion // Avg / avg(n.PropA))

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
