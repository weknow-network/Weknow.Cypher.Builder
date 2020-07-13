using System;
using System.Text;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Extensions
    /// </summary>
    public static class CypherExtensions
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
        public static Fluent Match(this Fluent p, IPattern pattern) => throw new NotImplementedException();

        #endregion // Match

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
        public static Fluent Create(this Fluent p, IPattern pattern) => throw new NotImplementedException();

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
        public static Fluent Merge(this Fluent p, IPattern pattern) => throw new NotImplementedException();

        #endregion // Merge

        #region OnCreateSet

        /// <summary>
        /// ON CREATE SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON CREATE &SET $1")]
        public static Fluent OnCreateSet(this Fluent p, IPropertiesOfType properties) => throw new NotImplementedException();

        /// <summary>
        /// ON CREATE SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="var">The variable.</param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON CREATE SET $1 = &$2")]
        public static Fluent OnCreateSet(this Fluent p, IVar var, IMap map) => throw new NotImplementedException();

        /// <summary>
        /// ON CREATE SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON CREATE &SET $1 = &$1")]
        public static Fluent OnCreateSet(this Fluent p, IMap map) => throw new NotImplementedException();

        #endregion // OnCreateSet

        #region OnMatchSet

        /// <summary>
        /// ON MATCH SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON MATCH &SET $1")]
        public static Fluent OnMatchSet(this Fluent p, IPropertiesOfType properties) => throw new NotImplementedException();

        /// <summary>
        /// ON MATCH SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="var">The variable.</param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON MATCH SET $1 = $2")]
        public static Fluent OnMatchSet(this Fluent p, IVar var, IMap map) => throw new NotImplementedException();

        /// <summary>
        /// ON MATCH SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON MATCH &SET $1 = &$1")]
        public static Fluent OnMatchSet(this Fluent p, IMap map) => throw new NotImplementedException();


        #endregion // OnMatchSet

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

        #endregion // OptionalMatch

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
        [Cypher("$0\r\nWHERE $1")]
        public static Fluent Where(this Fluent p, bool condition) => throw new NotImplementedException();

        /// <summary>
        /// WHERE phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// </example>
        [Cypher("$0\r\n&WHERE $1")]
        public static Fluent Where(this Fluent p, IPropertiesOfType properties) => throw new NotImplementedException();

        #endregion // Where

        #region Return

        /// <summary>
        /// RETURN phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// RETURN n
        /// </example>
        [Cypher("$0\r\n&RETURN $1")]
        public static Fluent Return(this Fluent p, params object[] vars) => throw new NotImplementedException();

        /// <summary>
        /// RETURN phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// RETURN n.Id
        /// </example>
        [Cypher("$0\r\n&RETURN $1")]
        public static Fluent Return(this Fluent p, IPropertiesOfType properties) => throw new NotImplementedException();

        #endregion // Return

        #region With

        /// <summary>
        /// WITH phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// WITH user, count(friend) AS friends
        /// WHERE friends > 10
        /// RETURN user
        /// </example>
        [Cypher("$0\r\nWITH $1")]
        public static Fluent With(this Fluent p, params object[] vars) => throw new NotImplementedException();

        #endregion // With

        #region OrderBy


        /// <summary>
        /// ORDER BY phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WITH user, count(friend) AS friends
        /// ORDER BY friends
        /// </example>
        [Cypher("$0\r\nORDER BY $1")]
        public static Fluent OrderBy(this Fluent p, params object[] vars) => throw new NotImplementedException();

        #endregion // OrderBy

        #region OrderByDesc

        /// <summary>
        /// ORDER BY DESC phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WITH user, count(friend) AS friends
        /// ORDER BY friends DESC
        /// </example>
        [Cypher("$0\r\nORDER BY $1 DESC")]
        public static Fluent OrderByDesc(this Fluent p, params object[] vars) => throw new NotImplementedException();

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

        #endregion // Limit

        #region Set

        /// <summary>
        /// SET label phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="node"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        /// <example>
        /// SSET n:Person
        /// </example>
        [Cypher("$0\r\nSET $1$2")]
        public static Fluent Set(this Fluent p, IVar node, ILabel label) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="node"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// SET n = $map
        /// </example>
        [Cypher("$0\r\n&SET $1 = $2")]
        public static Fluent Set(this Fluent p, IVar node, IVar map) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="node"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <example>
        /// SET n = $map
        /// </example>
        [Cypher("$0\r\n&SET $1 = $2")]
        public static Fluent Set(this Fluent p, IVar node, IMap map) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="map">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// SET n = $n
        /// </example>
        [Cypher("$0\r\n&SET $1 = &$1")]
        public static Fluent Set(this Fluent p, IMap map) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        /// <example>
        /// SET n.property1 = $value1,
        /// n.property2 = $value2
        /// </example>
        [Cypher("$0\r\nSET $1")]
        public static Fluent Set(this Fluent p, IVar node) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// SET n.property1 = $value1,
        /// n.property2 = $value2
        /// </example>
        [Cypher("$0\r\n&SET $1")]
        public static Fluent Set(this Fluent p, IPropertiesOfType properties) => throw new NotImplementedException();

        #endregion // Set

        #region Delete / Detach

        /// <summary>
        /// DELETE phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// DELETE n
        /// </example>
        [Cypher("$0\r\nDELETE $1")]
        public static Fluent Delete(this Fluent p, params object[] vars) => throw new NotImplementedException();

        /// <summary>
        /// DETACH DELETE phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// DETACH DELETE n
        /// </example>
        [Cypher("$0\r\nDETACH DELETE $1")]
        public static Fluent DetachDelete(this Fluent p, params object[] vars) => throw new NotImplementedException();

        #endregion // Delete / Detach

        #region Compare

        /// <summary>
        /// Compares the specified with.
        /// </summary>
        /// <param name="compare">The compare.</param>
        /// <param name="with">The with.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool Compare(
            this ReadOnlySpan<char> compare,
            ReadOnlySpan<char> with,
            bool ignoreCase = false)
        {
            if (compare.Length != with.Length)
                return false;
            for (int i = 0; i < compare.Length; i++)
            {
                if (ignoreCase)
                {
                    if (Char.ToLower(compare[i]) != Char.ToLower(with[i]))
                        return false;
                }
                else
                {
                    if (compare[i] != with[i])
                        return false;
                }
            }
            return true;
        }

        #endregion // Compare

        #region As

        /// <summary>
        /// Define variable as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("$0")]
        public static T As<T>(this IVar var) => throw new NotImplementedException();

        #endregion // As

        #region In

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="property">The property.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// n.property IN [$value1, $value2]
        /// </example>
        [Cypher("$0\\.$1 IN \\$$2")]
        public static bool In(this IVar variable, IProperty property, IVar compareWith) => throw new NotImplementedException();

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// n.property IN [$value1, $value2]
        /// </example>
        [Cypher("$0 IN \\$$1")]
        public static bool In(this IVar variable, IVar compareWith) => throw new NotImplementedException();

        #endregion // In

        // TODO: Avi review (ugly fix for the issue in tests [Relation_WithReuse_Test] & [Reuse_Complex5_Test])
        #region FixCypher

        /// <summary>
        /// Fixes the cypher is walk around for fixing illegal cypher.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <returns></returns>
        internal static StringBuilder FixCypher(this StringBuilder cypher) =>
                        cypher
                        .Replace("]--(", "]-(")
                        .Replace(")--[", ")-[")
                        .Replace(")<--[", ")<-[")
                        .Replace("]-->(", "]->(");

        #endregion // FixCypher
    }
}
