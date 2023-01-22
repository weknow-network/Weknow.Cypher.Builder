using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder;


/// <summary>
/// Cypher Extensions
/// </summary>
public static class CypherHigerAbstractionExtensions
{
    [Cypher("$0\r\n")]
    public static ICypherProc Proc(this ICypherStatement prev) => throw new NotImplementedException();

    #region SetDateConvention

    /// <summary>
    /// Set date convention.
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    [Cypher("$0\r\n\tON CREATE SET $1.`creation-date` = datetime()\r\n\tON MATCH SET $1.`modification-date` = datetime()")]
    [CypherClause]
    public static ICypherMergeStatement SetDateConvention(
        this ICypherMergeStatement prev,
        VariableDeclaration variable) => throw new NotImplementedException();

    /// <summary>
    /// Set date convention.
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="variable">The variable.</param>
    /// <returns></returns>
    [Cypher("$0\r\n\tSET $1.`creation-date` = datetime()")]
    [CypherClause]
    public static ICypherCreateStatement SetDateConvention(
        this ICypherCreateStatement prev,
        VariableDeclaration variable) => throw new NotImplementedException();

    #endregion // SetDateConvention
}
