using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder;

/// <summary>
/// Cypher Extensions
/// </summary>
public static partial class CypherExtensions
{
	#region Match

	/// <summary>
	/// Matches phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pp"></param>
	/// <returns></returns>
	/// <example>
	/// <![CDATA[ MATCH (n:Person)-[:KNOWS]->(m:Person) ]]>
	/// </example>
	[Cypher("$0\r\nMATCH $1")]
	[CypherClause]
	public static ICypherMatchStatement Match(this ICypherStatement p, ICypherStatement pp) => throw new NotImplementedException();

	/// <summary>
	/// Matches phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="patternattern">The patternattern.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example><![CDATA[ MATCH (n:Person)-[:KNOWS]->;(m:Person) ]]></example>
	[Cypher("$0\r\nMATCH $1")]
	[CypherClause]
	public static ICypherMatchStatement Match(this ICypherStatement p, IPattern patternattern) => throw new NotImplementedException();

	#endregion // Match

	#region OptionalMatch

	/// <summary>
	/// Optional Matches phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pp"></param>
	/// <returns></returns>
	/// <example>
	/// OPTIONAL MATCH (n:Person)-[:KNOWS]->(m:Person)
	/// </example>
	[Cypher("$0\r\nOPTIONAL MATCH $1")]
	[CypherClause]
	public static ICypherMatchStatement OptionalMatch(this ICypherStatement p, ICypherStatement pp) => throw new NotImplementedException();

	/// <summary>
	/// Matches phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pattern">The pattern.</param>
	/// <returns></returns>
	/// <example>
	/// <![CDATA[ MATCH (n:Person)-[:KNOWS]->;(m:Person) ]]>
	/// </example>
	[Cypher("$0\r\nOPTIONAL MATCH $1")]
	[CypherClause]
	public static ICypherMatchStatement OptionalMatch(this ICypherStatement p, IPattern pattern) => throw new NotImplementedException();

	#endregion // OptionalMatch

	#region Create

	/// <summary>
	/// Create phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pp"></param>
	/// <returns></returns>
	/// <example>
	/// CREATE (n {name: $value})
	/// </example>
	[Cypher("$0\r\n" +
		"CREATE $1")]
	[CypherClause]
	public static ICypherCreateStatement Create(this ICypherStatement p, ICypherStatement pp) => throw new NotImplementedException();

	/// <summary>
	/// Create phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pattern"></param>
	/// <returns></returns>
	/// <example>
	/// CREATE (n {name: $value})
	/// </example>
	[Cypher("$0\r\n" +
		"CREATE $1")]
	[CypherClause]
	public static ICypherCreateStatement Create(this ICypherStatement p, IPattern pattern) => throw new NotImplementedException();

	#endregion // Create

	#region Merge

	/// <summary>
	/// MERGE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pp"></param>
	/// <returns></returns>
	/// <example>
	/// MERGE (n:Person {name: $value})
	/// </example>
	[Cypher("$0\r\nMERGE $1")]
	[CypherClause]
	public static ICypherMergeStatement Merge(this ICypherStatement p, ICypherStatement pp) => throw new NotImplementedException();
	/// <summary>
	/// MERGE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="pattern">The pattern.</param>
	/// <returns></returns>
	/// <example>
	/// MERGE (n:Person {name: $value})
	/// </example>
	[Cypher("$0\r\nMERGE $1")]
	[CypherClause]
	public static ICypherMergeStatement Merge(this ICypherStatement p, IPattern pattern) => throw new NotImplementedException();

	#endregion // Merge

	#region OnCreateSet

	/// <summary>
	/// ON CREATE SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="cypher">
	/// Pure cypher injection.
	/// Should used for non-supported cypher extensions
	/// </param>
	/// <returns></returns>
	/// <example>
	/// .OnCreateSet(n, map)
	/// result in:
	/// ON CREATE SET n = map
	/// </example>
	[Obsolete("Should used for non-supported cypher extensions")]
	[Cypher("$0\r\n\tON CREATE &SET $1")]
	[CypherClause]
	public static ICypherMergeStatement OnCreateSet(this ICypherMergeStatement fluent, IRawCypher cypher) => throw new NotImplementedException();

	/// <summary>
	/// ON CREATE SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var"></param>
	/// <param name="assignment"></param>
	/// <returns></returns>
	/// <example>
	/// .OnCreateSet(n, map)
	/// result in:
	/// ON CREATE SET n = map
	/// </example>
	[Cypher("$0\r\n\tON CREATE &SET $1 = $2")]
	[CypherClause]
	public static ICypherMergeStatement OnCreateSet(this ICypherMergeStatement fluent, object var, object assignment) => throw new NotImplementedException();

	#endregion // OnCreateSet

	#region OnMatchSet

	/// <summary>
	/// ON MATCH SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="cypher">
	/// Pure cypher injection.
	/// Should used for non-supported cypher extensions
	/// </param>
	/// <returns></returns>
	/// <example>
	/// .OnMatchSet(n, map)
	/// result in:
	/// ON MATCH SET n = map
	/// </example>
	[Obsolete("Should used for non-supported cypher extensions")]
	[Cypher("$0\r\n\tON MATCH &SET $1")]
	[CypherClause]
	public static ICypherMergeStatement OnMatchSet(this ICypherMergeStatement fluent, IRawCypher cypher) => throw new NotImplementedException();

	/// <summary>
	/// ON MATCH SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var"></param>
	/// <param name="assignment"></param>
	/// <returns></returns>
	/// <example>
	/// .OnMatchSet(n, map)
	/// result in:
	/// ON MATCH SET n = map
	/// </example>
	[Cypher("$0\r\n\tON MATCH &SET $1 = $2")]
	[CypherClause]
	public static ICypherMergeStatement OnMatchSet(this ICypherMergeStatement fluent, object var, object assignment) => throw new NotImplementedException();

	#endregion // OnMatchSet

	#region OnMatchSetPlus

	/// <summary>
	/// ON MATCH SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var"></param>
	/// <param name="assignment"></param>
	/// <returns></returns>
	/// <example>
	/// .OnMatchSetPlus(n, map)
	/// result in:
	/// ON MATCH SET n += map
	/// </example>
	[Cypher("$0\r\n\tON MATCH &SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMergeStatement OnMatchSetPlus(this ICypherMergeStatement fluent, VariableDeclaration var, VariableDeclaration assignment) => throw new NotImplementedException();

	/// <summary>
	/// ON MATCH SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var"></param>
	/// <param name="assignment"></param>
	/// <returns></returns>
	/// <example>
	/// .OnMatchSetPlus(n, map)
	/// result in:
	/// ON MATCH SET n += $map
	/// </example>
	[Cypher("$0\r\n\tON MATCH &SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMergeStatement OnMatchSetPlus(this ICypherMergeStatement fluent, VariableDeclaration var, ParameterDeclaration assignment) => throw new NotImplementedException();

    #endregion // OnMatchSetPlus

    #region Set

    #region Set(ICypherMergeStatement ...)

    /// <summary>
    /// SET label.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    /// <example>
    /// SET n:Person:Manager
    /// </example>
    [Cypher("$0\r\n&SET $1$2")]
	[CypherClause]
	public static ICypherMergeStatement Set(this ICypherMergeStatement fluent, VariableDeclaration var, [CypherInputCollection] params ILabel[] label)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The complex.</param>
	/// <returns></returns>
	/// <example>
	/// .Set(n, map)
	/// result in:
	/// SET n = map
	/// </example>
	[Cypher("$0\r\n&SET $1 = $2")]
	[CypherClause]
	public static ICypherMergeStatement Set(this ICypherMergeStatement fluent, object var, object assignment)
		=> throw new NotImplementedException();

    #endregion // Set(ICypherMergeStatement ...)

    #region Set(ICypherMatchStatement ...)

    /// <summary>
    /// SET label.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    /// <example>
    /// SET n:Person:Manager
    /// </example>
    [Cypher("$0\r\n&SET $1$2")]
	[CypherClause]
	public static ICypherMatchStatement Set(this ICypherMatchStatement fluent, VariableDeclaration var, [CypherInputCollection] params ILabel[] label)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The complex.</param>
	/// <returns></returns>
	/// <example>
	/// .Set(n, map)
	/// result in:
	/// SET n = map
	/// </example>
	[Cypher("$0\r\n&SET $1 = $2")]
	[CypherClause]
	public static ICypherMatchStatement Set(this ICypherMatchStatement fluent, object var, object assignment)
		=> throw new NotImplementedException();

    #endregion // Set(ICypherMatchStatement ...)

    #region Set(ICypherCreateStatement ...)

    /// <summary>
    /// SET label.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    /// <example>
    /// SET n:Person:Manager
    /// </example>
    [Cypher("$0\r\n&SET $1$2")]
	[CypherClause]
	public static ICypherCreateStatement Set(this ICypherCreateStatement fluent, VariableDeclaration var, [CypherInputCollection] params ILabel[] label)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The complex.</param>
	/// <returns></returns>
	/// <example>
	/// .Set(n, map)
	/// result in:
	/// SET n = map
	/// </example>
	[Cypher("$0\r\n&SET $1 = $2")]
	[CypherClause]
	public static ICypherCreateStatement Set(this ICypherCreateStatement fluent, object var, object assignment)
		=> throw new NotImplementedException();

    #endregion // Set(ICypherCreateStatement ...)

    #region Set(ICypherRawStatement ...)

    /// <summary>
    /// SET label.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="label">The label.</param>
    /// <returns></returns>
    /// <example>
    /// SET n:Person:Manager
    /// </example>
    [Cypher("$0\r\n&SET $1$2")]
	[CypherClause]
	public static ICypherRawStatement Set(this ICypherRawStatement fluent, VariableDeclaration var, [CypherInputCollection] params ILabel[] label)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The complex.</param>
	/// <returns></returns>
	/// <example>
	/// .Set(n, map)
	/// result in:
	/// SET n = map
	/// </example>
	[Cypher("$0\r\n&SET $1 = $2")]
	[CypherClause]
	public static ICypherRawStatement Set(this ICypherRawStatement fluent, object var, object assignment)
		=> throw new NotImplementedException();

    #endregion // Set(ICypherRawStatement ...)

    #endregion // Set

    #region SetPlus

    #region SetPlus (ICypherMatchStatement...)

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The assignment.</param>
    /// <returns></returns>
    /// <example>
    /// .SetPlus(n, map)
    /// result in:
    /// SET n += map
    /// </example>
    [Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMatchStatement SetPlus(this ICypherMatchStatement fluent, VariableDeclaration var, VariableDeclaration assignment)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The assignment.</param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <example>
	/// .SetPlus(n, map)
	/// result in:
	/// SET n += $map
	/// </example>
	[Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMatchStatement SetPlus(this ICypherMatchStatement fluent, VariableDeclaration var, ParameterDeclaration assignment)
		=> throw new NotImplementedException();

    #endregion // SetPlus (ICypherMatchStatement...)

    #region SetPlus (ICypherMergeStatement...)

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The assignment.</param>
    /// <returns></returns>
    /// <example>
    /// .SetPlus(n, map)
    /// result in:
    /// SET n += map
    /// </example>
    [Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMergeStatement SetPlus(this ICypherMergeStatement fluent, VariableDeclaration var, VariableDeclaration assignment)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The assignment.</param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <example>
	/// .SetPlus(n, map)
	/// result in:
	/// SET n += $map
	/// </example>
	[Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherMergeStatement SetPlus(this ICypherMergeStatement fluent, VariableDeclaration var, ParameterDeclaration assignment)
		=> throw new NotImplementedException();

    #endregion // SetPlus (ICypherMergeStatement...)

    #region SetPlus (ICypherCreateStatement...)

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The assignment.</param>
    /// <returns></returns>
    /// <example>
    /// .SetPlus(n, map)
    /// result in:
    /// SET n += map
    /// </example>
    [Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherCreateStatement SetPlus(this ICypherCreateStatement fluent, VariableDeclaration var, VariableDeclaration assignment)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The assignment.</param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <example>
	/// .SetPlus(n, map)
	/// result in:
	/// SET n += $map
	/// </example>
	[Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherCreateStatement SetPlus(this ICypherCreateStatement fluent, VariableDeclaration var, ParameterDeclaration assignment)
		=> throw new NotImplementedException();

    #endregion // SetPlus (ICypherCreateStatement...)

    #region SetPlus (ICypherRawStatement...)

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The assignment.</param>
    /// <returns></returns>
    /// <example>
    /// .SetPlus(n, map)
    /// result in:
    /// SET n += map
    /// </example>
    [Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherRawStatement SetPlus(this ICypherRawStatement fluent, VariableDeclaration var, VariableDeclaration assignment)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <param name="assignment">The assignment.</param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <example>
	/// .SetPlus(n, map)
	/// result in:
	/// SET n += $map
	/// </example>
	[Cypher("$0\r\n&SET $1 \\+= $2")]
	[CypherClause]
	public static ICypherRawStatement SetPlus(this ICypherRawStatement fluent, VariableDeclaration var, ParameterDeclaration assignment)
		=> throw new NotImplementedException();

    #endregion // SetPlus (ICypherRawStatement...)

    #endregion // SetPlus

    #region SetAmbientLabels

    /// <summary>
    /// SET ambient labels phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <returns></returns>
    /// <example>
    /// .SetAmbient(n)
    /// result in:
    /// SET n:ENV_PROD
    /// </example>
    //[Cypher("$0\r\n&SET $1:")]
    public static ICypherCreateStatement SetAmbientLabels(this ICypherCreateStatement fluent, VariableDeclaration var)
		=> throw new NotImplementedException();


	/// <summary>
	/// SET ambient labels phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <returns></returns>
	/// <example>
	/// .SetAmbient(n)
	/// result in:
	/// SET n:ENV_PROD
	/// </example>
	//[Cypher("$0\r\n&SET $1:")]
	public static ICypherMatchStatement SetAmbientLabels(this ICypherMatchStatement fluent, VariableDeclaration var)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET ambient labels phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <returns></returns>
	/// <example>
	/// .SetAmbient(n)
	/// result in:
	/// SET n:ENV_PROD
	/// </example>
	//[Cypher("$0\r\n&SET $1:")]
	public static ICypherMergeStatement SetAmbientLabels(this ICypherMergeStatement fluent, VariableDeclaration var)
		=> throw new NotImplementedException();

	/// <summary>
	/// SET ambient labels phrase.
	/// </summary>
	/// <param name="fluent">The fluent.</param>
	/// <param name="var">The variable.</param>
	/// <returns></returns>
	/// <example>
	/// .SetAmbient(n)
	/// result in:
	/// SET n:ENV_PROD
	/// </example>
	//[Cypher("$0\r\n&SET $1:")]
	public static ICypherStatement SetAmbientLabels(this ICypherRawStatement fluent, VariableDeclaration var)
		=> throw new NotImplementedException();

	#endregion // SetAmbientLabels

	#region Where

	/// <summary>
	/// WHERE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="condition"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WHERE user.name = $name
	/// </example>
	[Cypher("$0\r\n&WHERE $1")]
	[CypherClause]
	public static ICypherMatchStatement Where(this ICypherMatchStatement p, bool condition) => throw new NotImplementedException();

	/// <summary>
	/// WHERE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="condition"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WHERE user.name = $name
	/// </example>
	[Cypher("$0\r\n&WHERE $1")]
	[CypherClause]
	public static ICypherStatement Where(this ICypherRawStatement p, bool condition) => throw new NotImplementedException();

	#endregion // Where

	#region Return

	/// <summary>
	/// RETURN phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// RETURN n
	/// </example>
	[Cypher("$0\r\n&RETURN $1")]
	[CypherClause]
	public static ICypherStatement Return(this ICypherStatement p, ParamsFirst<object?> var) => throw new NotImplementedException();

	/// <summary>
	/// RETURN phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// RETURN n
	/// </example>
	[Cypher("$0\r\n&RETURN $1$2")]
	[CypherClause]
	public static ICypherStatement Return(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	/// <summary>
	/// RETURN phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <returns></returns>
	/// <example>
	/// RETURN
	/// </example>
	[Cypher("$0\r\n&RETURN")]
	[CypherClause]
	public static ICypherStatement Return(this ICypherStatement p) => throw new NotImplementedException();

	#endregion // Return

	#region ReturnDistinct

	/// <summary>
	/// RETURN DISTINCT phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables.</param>
	/// <returns></returns>
	/// <example>
	/// RETURN DISTINCT n
	/// </example>
	[Cypher("$0\r\n&RETURN DISTINCT $1$2")]
	[CypherClause]
	public static ICypherStatement ReturnDistinct(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	#endregion // Return

	#region Union

	/// <summary>
	/// UNION phrase.
	/// </summary>
	/// <param name="p"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (a)-[:KNOWS]->(b)
	/// RETURN b.name
	/// UNION
	/// MATCH (a)-[:LOVES]->(b)
	/// RETURN b.name
	/// 
	/// Returns the distinct union of all query results. 
	/// Result column types and names have to match.
	/// </example>
	[Cypher("$0\r\nUNION")]
	[CypherClause]
	public static ICypherStatement Union(this ICypherStatement p) => throw new NotImplementedException();

	#endregion // Union

	#region Union All

	/// <summary>
	/// UNION ALL phrase.
	/// </summary>
	/// <param name="p"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (a)-[:KNOWS]->(b)
	/// RETURN b.name
	/// UNION ALL
	/// MATCH (a)-[:LOVES]->(b)
	/// RETURN b.name
	/// 
	/// Returns the distinct union of all query results. 
	/// Result column types and names have to match.
	/// </example>
	[Cypher("$0\r\nUNION ALL")]
	[CypherClause]
	public static ICypherStatement UnionAll(this ICypherStatement p) => throw new NotImplementedException();

	#endregion // Union All

	#region With

	/// <summary>
	/// WITH phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WHERE user.name = $name
	/// WITH user, count(friend) AS friends
	/// WHERE friends > 10
	/// RETURN user
	/// </example>
	[Cypher("$0\r\nWITH *")]
	[CypherClause]
	public static ICypherWithStatement With(this ICypherStatement p) => throw new NotImplementedException();

	/// <summary>
	/// WITH phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables</param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WHERE user.name = $name
	/// WITH user, count(friend) AS friends
	/// WHERE friends > 10
	/// RETURN user
	/// </example>
	[Cypher("$0\r\nWITH $1$2")]
	[CypherClause]
	public static ICypherWithStatement With(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	#endregion // With

	#region Unwind

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <param name="next">The next statement.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND \\$$[]1 AS $2\r\n$3")]
	[CypherClause]
	[Obsolete("use the overload with FluentUnwindAction")]
	public static ICypherStatement Unwind<T>(this ICypherStatement p,
									[CypherInputCollection] IEnumerable<T> items,
									VariableDeclaration item,
									ICypherStatement next) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND \\$$[]1 AS $2")]
	[CypherClause]
	public static ICypherStatement Unwind<T>(this ICypherStatement p,
									[CypherInputCollection] IEnumerable<T> items,
									FluentUnwindAction item) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND $1 AS $2")]
	[CypherClause]
	public static ICypherStatement Unwind(this ICypherStatement p,
								VariableDeclaration items,
								FluentUnwindAction item) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND $1 AS $2")]
	[CypherClause]
	public static ICypherStatement Unwind<T>(this ICypherStatement p,
								VariableDeclaration<T> items,
								FluentUnwindAction<T> item) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND $1 AS $2")]
	[CypherClause]
	public static ICypherStatement Unwind(this ICypherStatement p,
								ParameterDeclaration items,
								FluentUnwindAction item) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND $1 AS $2")]
	[CypherClause]
	public static ICypherStatement Unwind<T>(this ICypherStatement p,
								ParameterDeclaration<T> items,
								FluentUnwindAction<T> item) => throw new NotImplementedException();

	/// <summary>
	/// UNWIND phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <param name="next">The next statement.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <example>
	/// UNWIND $names AS name
	/// MATCH(n { name: name})
	/// RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&UNWIND $1 AS $2\r\n$3")]
	[CypherClause]
	[Obsolete("use the overload with FluentUnwindAction")]
	public static ICypherStatement Unwind(this ICypherStatement p,
								VariableDeclaration items,
								VariableDeclaration item,
								ICypherStatement next) => throw new NotImplementedException();

	/// <summary>
	/// Unwinds the specified items.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="items">The items.</param>
	/// <param name="item">The item.</param>
	/// <param name="next">The next statement.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	/// <exception cref="NotImplementedException"></exception>
	[Cypher("$0\r\n&UNWIND $1 AS $2\r\n$3")]
	[CypherClause]
	[Obsolete("use the overload with FluentUnwindAction")]
	public static ICypherStatement Unwind(this ICypherStatement p,
									ParameterDeclaration items,
									VariableDeclaration item,
									ICypherStatement next) => throw new NotImplementedException();

	#endregion // Unwind

	#region Foreach

	#region deprecated


	/// <summary>
	/// FOREACH phrase.
	/// </summary>
	/// <param name="prev">The previous.</param>
	/// <param name="item">The item.</param>
	/// <param name="items">The items.</param>
	/// <param name="p">The p.</param>
	/// <returns></returns>
	/// <example>
	/// FOREACH $names IN name |
	///     MATCH(n { name: name})
	///     RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&FOREACH ($1 IN $2 |\r\n\t$3)")]
	[CypherClause]
    [Obsolete("use the overload with FluentForEachAction")]
    public static ICypherStatement Foreach(
		this ICypherStatement prev,
		VariableDeclaration item,
		VariableDeclaration items,
		ICypherStatement p) => throw new NotImplementedException();


	/// <summary>
	/// FOREACH phrase.
	/// </summary>
	/// <param name="prev">The previous.</param>
	/// <param name="item">The item.</param>
	/// <param name="items">The items.</param>
	/// <param name="p">The p.</param>
	/// <returns></returns>
	/// <example>
	/// FOREACH $names IN name |
	///     MATCH(n { name: name})
	///     RETURN avg(n.age)
	/// </example>
	[Cypher("$0\r\n&FOREACH ($1 IN $2 |\r\n\t$3)")]
	[CypherClause]
    [Obsolete("use the overload with FluentForEachAction")]
    public static ICypherStatement Foreach(
		this ICypherStatement prev,
		VariableDeclaration item,
		ParameterDeclaration items,
		ICypherStatement p) => throw new NotImplementedException();

	#endregion // deprecated

	/// <summary>
	/// FOREACH phrase.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prev">The previous.</param>
	/// <param name="items">The items.</param>
	/// <param name="iteration">The iteration expression.</param>
	/// <returns></returns>
	/// <example>
	/// FOREACH $names IN name |
	///     MATCH(n { name: name})
	///     RETURN avg(n.age)
	/// </example>
	//[Cypher("$0\r\n&FOREACH ($1 IN $[]2 |\r\n\t$3)")]
	[CypherClause]
    //[Obsolete("Not implemented yet", true)]
    public static ICypherStatement Foreach<T>(
		this ICypherStatement prev,
		[CypherInputCollection] IEnumerable<T> items, 
		FluentForEachAction<T> iteration) => throw new NotImplementedException();

    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    ///     MATCH(n { name: name})
    ///     RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach(
		this ICypherStatement prev,
		VariableDeclaration items,
		FluentForEachAction iteration) => throw new NotImplementedException();


    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    ///     MATCH(n { name: name})
    ///     RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach(
		this ICypherStatement prev,
		ParameterDeclaration items,
		FluentForEachAction iteration) => throw new NotImplementedException();

    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach<T>(
		this ICypherStatement prev,
		VariableDeclaration<T> items,
		FluentForEachAction<T> iteration) => throw new NotImplementedException();


    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach<T>(
		this ICypherStatement prev,
		ParameterDeclaration<T> items,
		FluentForEachAction<T> iteration) => throw new NotImplementedException();


    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach(
		this ICypherStatement prev,
        ICypherStatement items,
		FluentForEachAction iteration) => throw new NotImplementedException();

    /// <summary>
    /// FOREACH phrase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prev">The previous.</param>
    /// <param name="items">The items.</param>
    /// <param name="iteration">The iteration expression.</param>
    /// <returns></returns>
    /// <example>
    /// FOREACH $names IN name |
    /// MATCH(n { name: name})
    /// RETURN avg(n.age)
    /// </example>
    //[Cypher("$0\r\n&FOREACH ($2 IN $1)")]
	[CypherClause]
	public static ICypherStatement Foreach<T>(
		this ICypherStatement prev,
        ICypherStatement items,
		FluentForEachAction<T> iteration) => throw new NotImplementedException();

	#endregion // Foreach

	#region OrderBy


	/// <summary>
	/// ORDER BY phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables</param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WITH user, count(friend) AS friends
	/// ORDER BY friends
	/// </example>
	[Cypher("$0\r\nORDER BY $1$2")]
	[CypherClause]
	public static ICypherStatement OrderBy(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	#endregion // OrderBy

	#region OrderByDesc

	/// <summary>
	/// ORDER BY DESC phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables</param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// WITH user, count(friend) AS friends
	/// ORDER BY friends DESC
	/// </example>
	[Cypher("$0\r\nORDER BY $1$2 DESC")]
	[CypherClause]
	public static ICypherStatement OrderByDesc(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	#endregion // OrderByDesc

	#region Skip

	/// <summary>
	/// SKIP phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="count"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// SKIP 10
	/// LIMIT 5
	/// </example>
	[Cypher("$0\r\nSKIP $1")]
	[CypherClause]
	public static ICypherStatement Skip(this ICypherStatement p, int count) => throw new NotImplementedException();

	/// <summary>
	/// SKIP phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="count"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// SKIP 10
	/// LIMIT 5
	/// </example>
	[Cypher("$0\r\nSKIP $1")]
	[CypherClause]
	public static ICypherStatement Skip(this ICypherStatement p, ParameterDeclaration count) => throw new NotImplementedException();

	#endregion // Skip

	#region Limit

	/// <summary>
	/// LIMIT phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="count"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// SKIP 10
	/// LIMIT 5
	/// </example>
	[Cypher("$0\r\nLIMIT $1")]
	[CypherClause]
	public static ICypherStatement Limit(this ICypherStatement p, int count) => throw new NotImplementedException();

	/// <summary>
	/// LIMIT phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="count"></param>
	/// <returns></returns>
	/// <example>
	/// MATCH (user)-[:FRIEND]-(friend)
	/// SKIP 10
	/// LIMIT 5
	/// </example>
	[Cypher("$0\r\nLIMIT $1")]
	[CypherClause]
	public static ICypherStatement Limit(this ICypherStatement p, ParameterDeclaration count) => throw new NotImplementedException();

	#endregion // Limit

	#region Delete / Detach

	/// <summary>
	/// DELETE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables</param>
	/// <returns></returns>
	/// <example>
	/// DELETE n
	/// </example>
	[Cypher("$0\r\nDELETE $1$2")]
	[CypherClause]
	public static ICypherStatement Delete(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	/// <summary>
	/// DETACH DELETE phrase.
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The first variable.</param>
	/// <param name="vars">Rest of the variables</param>
	/// <returns></returns>
	/// <example>
	/// DETACH DELETE n
	/// </example>
	[Cypher("$0\r\nDETACH DELETE $1$2")]
	[CypherClause]
	public static ICypherStatement DetachDelete(this ICypherStatement p, ParamsFirst<object?> var, [CypherInputCollection] params object?[] vars) => throw new NotImplementedException();

	#endregion // Delete / Detach

	#region WithRawCypher

	/// <summary>
	/// Pure cypher injection.
	/// Should used for non-supported cypher extensions
	/// </summary>
	/// <param name="p">previous part</param>
	/// <param name="cypher">The cypher.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	[Cypher("$0\r\n$1")]
	[CypherClause]
	[Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
	public static ICypherRawStatement WithRawCypher(this ICypherStatement p, RawCypher cypher) => throw new NotImplementedException();

	#endregion // WithRawCypher

	#region CASE

	/// <summary>
	/// CASE WHEN ELSE END
	/// https://neo4j.com/docs/cypher-manual/5/syntax/expressions/
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="var">The variable.</param>
	/// <example>
	/// <![CDATA[
	/// CASE n.eyes
	///   WHEN 'blue' THEN 1
	///   WHEN 'brown' THEN 2
	///   ELSE 3
	/// END
	/// ]]>  
	/// </example>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	[Cypher("$0\r\nCASE $1")]
	[CypherClause]
	public static FluentCase Case(this ICypherStatement p, object var) => throw new NotImplementedException();

	/// <summary>
	/// CASE WHEN ELSE END
	/// https://neo4j.com/docs/cypher-manual/5/syntax/expressions/
	/// </summary>
	/// <param name="p">The p.</param>
	/// <example>
	/// <![CDATA[ 
	/// CASE
	///     WHEN n.eyes = 'blue' THEN 1
	///     WHEN n.age< 40 THEN 2
	///     ELSE 3
	/// END   
	/// 
	/// ---
	/// 
	/// MATCH(n)-[r]->(m)
	/// RETURN
	/// CASE
	///   WHEN n:A&B THEN 1
	///   WHEN r:!R1&!R2 THEN 2
	///   ELSE -1
	/// END AS result   
	/// ]]>  
	/// </example>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	[Cypher("$0\r\nCASE")]
	[CypherClause]
	public static FluentCase Case(this ICypherStatement p) => throw new NotImplementedException();

	#endregion // CASE

	#region When

	[Cypher("$0\r\n\t&WHEN $1")]
	[CypherClause]
	public static FluentCaseWhen When(this FluentCase prv, object condition) => throw new NotImplementedException();

	[Cypher("$0\r\n\t&WHEN '$1'")]
	[CypherClause]
	public static FluentCaseWhen When(this FluentCase prv, string condition) => throw new NotImplementedException();

	[Cypher("$0\r\n\t&WHEN $1")]
	[CypherClause]
	public static FluentCaseWhen When(this FluentCase prv, bool condition) => throw new NotImplementedException();


	[Cypher("$0\r\n\t&WHEN $1$2")]
	[CypherClause]
	public static FluentCaseWhen When(this FluentCase prv, VariableDeclaration var, ILabel label) => throw new NotImplementedException();

	[Cypher("$0\r\n\t&WHEN $1$2")]
	[CypherClause]
	public static FluentCaseWhen When(this FluentCase prv, VariableDeclaration var, IType type) => throw new NotImplementedException();

	#endregion // When

	#region Then

	[Cypher("$0 THEN $1")]
	[CypherClause]
	public static FluentCase Then(this FluentCaseWhen prv, object? value) => throw new NotImplementedException();

	[Cypher("$0 THEN '$1'")]
	[CypherClause]
	public static FluentCase Then(this FluentCaseWhen prv, string value) => throw new NotImplementedException();

	#endregion // Then

	#region Else

	[Cypher("$0\r\n\tELSE $1")]
	[CypherClause]
	public static FluentCase Else(this FluentCase prv, object? value) => throw new NotImplementedException();

	[Cypher("$0\r\n\tELSE '$1'")]
	[CypherClause]
	public static FluentCase Else(this FluentCase prv, string value) => throw new NotImplementedException();

	#endregion // Else

	#region End

	[Cypher("$0\r\nEND")]
	[CypherClause]
	public static ICypherStatement End(this FluentCase prv) => throw new NotImplementedException();

	#endregion // End

	#region IgnoreAmbient

	//[Cypher("$0\r\n$1")]
	/// <summary>
	/// Avoid adding ambient labels within this scope
	/// </summary>
	/// <param name="p">The p.</param>
	/// <param name="next">The next.</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	public static ICypherStatement NoAmbient(this ICypherStatement p, ICypherStatement next) => throw new NotImplementedException();

	#endregion // IgnoreAmbient

	#region IsNull

	/// <summary>
	/// Use to generate IS NULL statement (cypher)
	/// </summary>
	/// <param name="instance">The instance.</param>
	/// <returns></returns>
	[Cypher("$0 IS NULL")]
	public static bool IsNull(this object instance) => instance == null;

	#endregion // IsNull

	#region IsNotNull

	/// <summary>
	/// Use to generate IS NOT NULL statement (cypher)
	/// </summary>
	/// <param name="instance">The instance.</param>
	/// <returns></returns>
	[Cypher("$0 IS NOT NULL")]
	public static bool IsNotNull(this object instance) => instance == null;

	#endregion // IsNotNull
}
