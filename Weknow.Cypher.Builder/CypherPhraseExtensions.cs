﻿using System;
using System.Text;

using Weknow.GraphDbCommands.Declarations;

using static Weknow.GraphDbCommands.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.GraphDbCommands;

/// <summary>
/// Cypher Extensions
/// </summary>
public static class CypherPhraseExtensions
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
    public static Fluent Match(this Fluent p, Fluent pp) => throw new NotImplementedException();

    /// <summary>
    /// Matches phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns></returns>
    /// <example>
    /// <![CDATA[ MATCH (n:Person)-[:KNOWS]->;(m:Person) ]]>
    /// </example>
    [Cypher("$0\r\nMATCH $1")]
    public static Fluent Match(this Fluent p, IPattern patternattern) => throw new NotImplementedException();

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
    public static Fluent OptionalMatch(this Fluent p, Fluent pp) => throw new NotImplementedException();

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
    public static Fluent OptionalMatch(this Fluent p, IPattern patternattern) => throw new NotImplementedException();

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
    public static Fluent Create(this Fluent p, Fluent pp) => throw new NotImplementedException();

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
    public static Fluent Create(this Fluent p, IPattern patternattern) => throw new NotImplementedException();

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
    public static Fluent Merge(this Fluent p, Fluent pp) => throw new NotImplementedException();
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
    public static Fluent Merge(this Fluent p, IPattern patternattern) => throw new NotImplementedException();

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
    public static Fluent OnCreateSet(this Fluent fluent, IRawCypher cypher) => throw new NotImplementedException();

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
    public static Fluent OnCreateSet(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment) => throw new NotImplementedException();

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
    /// ON CREATE SET n = $map
    /// </example>
    [Cypher("$0\r\n\tON CREATE &SET $1 = $2")]
    public static Fluent OnCreateSet(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment) => throw new NotImplementedException();

    /// <summary>
    /// ON CREATE SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .OnCreateSet(n, new {prm._.Name, var._.Code})
    /// result in:
    /// ON CREATE SET n.Name = $Name, n.Code = prm.Code
    /// </example>
    [Cypher("$0\r\n\tON CREATE &SET +01$2")]
    public static Fluent OnCreateSet(this Fluent fluent, VariableDeclaration var, object assignment) => throw new NotImplementedException();

    #endregion // OnCreateSet

    #region OnCreateSetPlus

    /// <summary>
    /// ON CREATE SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .OnCreateSetPlus(n, map)
    /// result in:
    /// ON CREATE SET n += map
    /// </example>
    [Cypher("$0\r\n\tON CREATE &SET $1 = $2")]
    public static Fluent OnCreateSetPlus(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment) => throw new NotImplementedException();

    /// <summary>
    /// ON CREATE SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .OnCreateSetPlus(n, map)
    /// result in:
    /// ON CREATE SET n += $map
    /// </example>
    [Cypher("$0\r\n\tON CREATE &SET $1 = $2")]
    public static Fluent OnCreateSetPlus(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment) => throw new NotImplementedException();

    #endregion // OnCreateSetPlus

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
    public static Fluent OnMatchSet(this Fluent fluent, IRawCypher cypher) => throw new NotImplementedException();

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
    public static Fluent OnMatchSet(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment) => throw new NotImplementedException();

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
    /// ON MATCH SET n = $map
    /// </example>
    [Cypher("$0\r\n\tON MATCH &SET $1 = $2")]
    public static Fluent OnMatchSet(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment) => throw new NotImplementedException();

    /// <summary>
    /// ON MATCH SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .OnMatchSet(n, new {prm._.Name, var._.Code})
    /// result in:
    /// ON MATCH SET n.Name = $Name, n.Code = prm.Code
    /// </example>
    [Cypher("$0\r\n\tON MATCH &SET +01$2")]
    public static Fluent OnMatchSet(this Fluent fluent, VariableDeclaration var, object assignment) => throw new NotImplementedException();

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
    public static Fluent OnMatchSetPlus(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment) => throw new NotImplementedException();

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
    [Cypher("$0\r\n\tON MATCH &SET $1 = $2")]
    public static Fluent OnMatchSetPlus(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment) => throw new NotImplementedException();

    #endregion // OnMatchSetPlus

    #region Set

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
    public static Fluent Set(this Fluent fluent, VariableDeclaration var, params ILabel[] label)
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
    public static Fluent Set(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment)
        => throw new NotImplementedException();

    /// <summary>
    /// SET phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var"></param>
    /// <param name="assignment"></param>
    /// <returns></returns>
    /// <example>
    /// .Set(n, map)
    /// result in:
    /// SET n = $map
    /// </example>
    [Cypher("$0\r\n&SET $1 = $2")]
    public static Fluent Set(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment)
        => throw new NotImplementedException();

    /// <summary>
    /// SET  phrase.
    /// </summary>
    /// <param name="fluent">The fluent.</param>
    /// <param name="var">The variable.</param>
    /// <param name="assignment">The complex.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <example>
    /// .Set(n, new {prm._.Name, var._.Code})
    /// result in:
    /// SET n.Name = $Name, n.Code = prm.Code
    /// </example>
    [Cypher("$0\r\n&SET +01$2")]
    public static Fluent Set(this Fluent fluent, VariableDeclaration var, object assignment)
        => throw new NotImplementedException();

    #endregion // Set

    #region SetPlus

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
    public static Fluent SetPlus(this Fluent fluent, VariableDeclaration var, VariableDeclaration assignment)
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
    public static Fluent SetPlus(this Fluent fluent, VariableDeclaration var, ParameterDeclaration assignment)
        => throw new NotImplementedException();

    #endregion // SetPlus

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
    public static Fluent Where(this Fluent p, bool condition) => throw new NotImplementedException();

    #endregion // Where

    #region Return

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
    public static Fluent Return(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent ReturnDistinct(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent Union(this Fluent p) => throw new NotImplementedException();

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
    public static Fluent UnionAll(this Fluent p) => throw new NotImplementedException();

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
    public static Fluent With(this Fluent p) => throw new NotImplementedException();

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
    public static Fluent With(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent Unwind<T>(this Fluent p, 
                                    IEnumerable<T> items, 
                                    VariableDeclaration item, 
                                    Fluent next) => throw new NotImplementedException();


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
    [Cypher("$0\r\n&UNWIND \\$$1 AS $2\r\n$3")]
    public static Fluent Unwind(this Fluent p, 
                                VariableDeclaration items, 
                                VariableDeclaration item, 
                                Fluent next) => throw new NotImplementedException();

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
    public static Fluent Unwind(this Fluent p, 
                                    ParameterDeclaration items,
                                    VariableDeclaration item, 
                                    Fluent next) => throw new NotImplementedException();

    #endregion // Unwind

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
    public static Fluent OrderBy(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent OrderByDesc(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent Skip(this Fluent p, int count) => throw new NotImplementedException();

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
    public static Fluent Skip(this Fluent p, ParameterDeclaration count) => throw new NotImplementedException();

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
    public static Fluent Limit(this Fluent p, int count) => throw new NotImplementedException();

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
    public static Fluent Limit(this Fluent p, ParameterDeclaration count) => throw new NotImplementedException();

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
    public static Fluent Delete(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

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
    public static Fluent DetachDelete(this Fluent p, ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    #endregion // Delete / Detach

    #region WithRawCypher

    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    /// <param name="cypher">The cypher.</param>
    /// <returns></returns>
    [Cypher("$0\r\n$1")]
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    public static Fluent WithRawCypher(this Fluent p, RawCypher cypher) => throw new NotImplementedException();

    #endregion // WithRawCypher

    #region DropConstraint

    /// <summary>
    /// Drop a constraint.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("$0\r\nDROP CONSTRAINT $1")]
    public static Fluent DropConstraint(
        this Fluent p,
        string name) => throw new NotImplementedException();

    #endregion // DropConstraint

    #region TryDropConstraint

    /// <summary>
    /// Drop a constraint phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("$0\r\nDROP CONSTRAINT $1 IF NOT EXISTS")]
    public static Fluent TryDropConstraint(
        this Fluent p,
        string name) => throw new NotImplementedException();


    #endregion // TryDropConstraint

    #region CreateConstraint

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE CONSTRAINT $1\r\n\tFOR $2\r\n\tREQUIRE ($3$4)")]
    public static Fluent CreateConstraint(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="vars">The vars.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE CONSTRAINT $1\r\n\tFOR $2\r\n\tREQUIRE ($3) $4")]
    public static Fluent CreateConstraint(
        this Fluent p,
        string name,
        IPattern pattern,
        IEnumerable<object> vars,
        ConstraintType type) => throw new NotImplementedException();

    #endregion // CreateConstraint

    #region TryCreateConstraint

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE CONSTRAINT $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tREQUIRE ($3$4)")]
    public static Fluent TryCreateConstraint(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="vars">The vars.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE CONSTRAINT $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tREQUIRE ($3) $4")]
    public static Fluent TryCreateConstraint(
        this Fluent p,
        string name,
        IPattern pattern,
        IEnumerable<object> vars,
        ConstraintType type) => throw new NotImplementedException();

    #endregion // TryCreateConstraint

    #region DropIndex

    /// <summary>
    /// Drop a index.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("$0\r\nDROP CONSTRAINT $1")]
    public static Fluent DropIndex(
        this Fluent p,
        string name) => throw new NotImplementedException();

    #endregion // DropIndex

    #region TryDropIndex

    /// <summary>
    /// Drop a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [Cypher("$0\r\nDROP CONSTRAINT $1 IF NOT EXISTS")]
    public static Fluent TryDropIndex(
        this Fluent p,
        string name) => throw new NotImplementedException();


    #endregion // TryDropIndex

    #region Create..Index

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE INDEX $1\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent CreateIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a BTREE index on nodes with label and property
    /// with an index provider.
    /// The other index settings will have their default values.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <remarks>
    /// Options are not included, make sure to add the proper ones, for example:
    /// OPTIONS {
    /// indexProvider: 'native-btree-1.0',
    /// indexConfig: {
    /// `spatial.cartesian.min`: [-100.0, -100.0],
    /// `spatial.cartesian.max`: [100.0, 100.0]
    /// }
    /// }
    /// </remarks>
    [Cypher("$0\r\nCREATE BTREE INDEX $1\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent CreateBTreeIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a TEXT index on nodes with label Person and property name.
    /// The property value type should be a string for the TEXT index.
    /// Other value types are ignored by the TEXT index.
    /// TEXT index is utilized if the predicate compares the property with a string.
    /// Note that for example:
    /// toLower(n.name) = 'Example String'
    /// does not use an index.
    /// TEXT index is utilized to check the IN list checks,
    /// when all elements in the list are strings.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("$0\r\nCREATE TEXT INDEX $1\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent CreateTextIndex(
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a full-text index on relationships with the name index_name and analyzer.
    /// Full-text indexes on relationships can only be used by from
    /// the procedure db.index.fulltext.queryRelationships.
    /// The other index settings will have their default values.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="p">The p.</param>
    /// <param name="analyzer">The analyzer.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE FULLTEXT INDEX $1\r\n\tFOR $2\r\n\tON EACH ($3$4)\r\n\t OPTIONS {\r\n\t\tindexConfig: {\r\n\t\t\t`fulltext.analyzer`: '$5'\r\n\t\t  }\r\n\t}")]
    public static Fluent CreateFullTextIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        FullTextAnalyzer analyzer,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    #endregion // Create..Index

    #region TryCreate..Index

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    [Cypher("$0\r\nCREATE INDEX $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent TryCreateIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <remarks>
    /// Options are not included, make sure to add the proper ones, for example:
    /// OPTIONS {
    /// indexProvider: 'native-btree-1.0',
    /// indexConfig: {
    /// `spatial.cartesian.min`: [-100.0, -100.0],
    /// `spatial.cartesian.max`: [100.0, 100.0]
    /// }
    /// }
    /// </remarks>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("$0\r\nCREATE BTREE INDEX $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent TryCreateBTreeIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    /// <summary>
    /// Create a index phrase.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="">The .</param>
    /// <param name="analyzer">The analyzer.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("$0\r\nCREATE FULLTEXT INDEX $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tON EACH ($3$4)\r\n\t OPTIONS {\r\n\t\tindexConfig: {\r\n\t\t\t`fulltext.analyzer`: '$5'\r\n\t\t  }\r\n\t}")]
    public static Fluent TryCreateFullTextIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        FullTextAnalyzer analyzer,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();


    /// <summary>
    /// Create a TEXT index on nodes with label Person and property name.
    /// The property value type should be a string for the TEXT index.
    /// Other value types are ignored by the TEXT index.
    /// TEXT index is utilized if the predicate compares the property with a string.
    /// Note that for example:
    /// toLower(n.name) = 'Example String'
    /// does not use an index.
    /// TEXT index is utilized to check the IN list checks,
    /// when all elements in the list are strings.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <param name="name">The name.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="var">The variable.</param>
    /// <param name="vars">The vars.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CREATE (n {name: $value})
    /// </example>
    [Cypher("$0\r\nCREATE TEXT INDEX $1 IF NOT EXISTS\r\n\tFOR $2\r\n\tON ($3$4)")]
    public static Fluent TryCreateTextIndex(
        this Fluent p,
        string name,
        IPattern pattern,
        ParamsFirst<object> var, params object[] vars) => throw new NotImplementedException();

    #endregion // TryCreate..Index

    //#region Coalesce / coalesce(n)

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
    //                    object variable, 
    //                    object defaultValue) => throw new NotImplementedException();

    //#endregion // Coalesce / coalesce(n)
}
