using Weknow.CypherBuilder.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CypherBuilder;

/// <summary>
/// Entry point for constructing root level Cypher.
/// For fluent cypher check <see cref="CypherExtensions" />
/// </summary>
public partial interface ICypher
{
    partial interface IFn
    {
        /// <summary>
        /// Aggregation's functions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICypherAggregation Ag => throw new NotImplementedException();
        /// <summary>
        /// Aggregation's functions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICypherAggregation Aggregation => throw new NotImplementedException();
    }

    public interface ICypherAggregation
    {
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
        public VariableDeclaration Sum(object prop) => throw new NotImplementedException();

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
        public VariableDeclaration Max(object prop) => throw new NotImplementedException();

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
        public VariableDeclaration Min(object prop) => throw new NotImplementedException();

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
        public VariableDeclaration Avg(object prop) => throw new NotImplementedException();

        #endregion // Avg / avg(n.PropA)) 

        #region Sum / sum(DISTINCT n.PropA))

        /// <summary>
        /// Sum numerical values. Similar functions are avg(), min(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN sum(n.PropA)
        /// </example>
        [Cypher("sum(DISTINCT $0)")]
        public VariableDeclaration SumDistinct(object prop) => throw new NotImplementedException();

        #endregion // Sum / sum(DISTINCT n.PropA))

        #region Max / max(DISTINCT n.PropA))

        /// <summary>
        /// Max numerical values. Similar functions are avg(), min(), sum().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN max(n.PropA)
        /// </example>
        [Cypher("max(DISTINCT $0)")]
        public VariableDeclaration MaxDistinct(object prop) => throw new NotImplementedException();

        #endregion // Max / max(DISTINCT n.PropA))

        #region Min / min(DISTINCT n.PropA))

        /// <summary>
        /// Min numerical values. Similar functions are avg(), sum(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN min(n.PropA)
        /// </example>
        [Cypher("min(DISTINCT $0)")]
        public VariableDeclaration MinDistinct(object prop) => throw new NotImplementedException();

        #endregion // Min / min(DISTINCT n.PropA))

        #region Avg / avg(DISTINCT n.PropA))

        /// <summary>
        /// Avg numerical values. Similar functions are sum(), min(), max().
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN avg(n.PropA)
        /// </example>
        [Cypher("avg(DISTINCT $0)")]
        public VariableDeclaration AvgDistinct(object prop) => throw new NotImplementedException();

        #endregion // Avg / avg(DISTINCT n.PropA)) 

        #region Count / count(n)

        /// <summary>
        /// Count the results.
        /// </summary>
        /// <param name="result">The result to be count.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN count(n)
        /// </example>
        [Cypher("count($0)")]
        public VariableDeclaration Count(object result) => throw new NotImplementedException();

        #endregion // Count / count(n)


        #region CountDistinct / count(DISTINCT n)

        /// <summary>
        /// Count the results.
        /// </summary>
        /// <param name="result">The result to be count.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN count(n)
        /// </example>
        [Cypher("count(DISTINCT $0)")]
        public VariableDeclaration CountDistinct(object result) => throw new NotImplementedException();

        #endregion // CountDistinct / count(DISTINCT n)
    }
}
