using Weknow.CypherBuilder.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CypherBuilder;

/// <summary>
/// Entry point for constructing root level Cypher.
/// For fluent cypher check <see cref="CypherExtensions" />
/// </summary>
public partial interface ICypher
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
}