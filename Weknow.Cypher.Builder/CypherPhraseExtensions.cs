using System;
using System.Text;

using Weknow.Cypher.Builder.Declarations;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
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
        public static Fluent Match(this Fluent p, IPattern pattern) => throw new NotImplementedException();

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
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON CREATE &SET $1 = &$1")]
        public static Fluent OnCreateSet(this Fluent p, ISetExpression exp) => throw new NotImplementedException();

        #endregion // OnCreateSet

        #region OnMatchSet

        /// <summary>
        /// ON MATCH SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {id: $value})
        /// ON CREATE SET p = $map
        /// </example>
        [Cypher("$0\r\n\tON MATCH &SET $1")]
        public static Fluent OnMatchSet(this Fluent p, ISetExpression exp) => throw new NotImplementedException();

        #endregion // OnMatchSet

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
        [Obsolete("Wrong syntax")]
        public static Fluent Where(this Fluent p, IProperties properties) => throw new NotImplementedException();

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
        /// <example>
        /// RETURN n.Id
        /// </example>
        [Cypher("$0\r\n&RETURN $1")]
        public static Fluent Return(this Fluent p, IProperties properties) => throw new NotImplementedException();

        #endregion // Return

        #region ReturnDistinct

        /// <summary>
        /// RETURN DISTINCT phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="vars"></param>
        /// <returns></returns>
        /// <example>
        /// RETURN DISTINCT n
        /// </example>
        [Cypher("$0\r\n&RETURN DISTINCT $1")]
        public static Fluent ReturnDistinct(this Fluent p, params object[] vars) => throw new NotImplementedException();

        /// <summary>
        /// RETURN phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// RETURN n.Id
        /// </example>
        [Cypher("$0\r\n&RETURN DISTINCT $1")]
        public static Fluent ReturnDistinct(this Fluent p, IProperties properties) => throw new NotImplementedException();

        #endregion // Return

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
        /// SET label.
        /// </summary>
        /// <param name="fluent">The fluent.</param>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// SSET n:Person
        /// </example>
        [Cypher("$0\r\n&SET $1:$2")]
        public static Fluent Set(this Fluent fluent, VariableDeclaration var, params ILabel[] label)
            => throw new NotImplementedException() ;

        /// <summary>
        /// SET  phrase.
        /// </summary>
        /// <param name="fluent">The fluent.</param>
        /// <param name="complex">The complex.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// SSET n:Person
        /// </example>
        [Cypher("$0\r\n&SET $1")]
        public static Fluent Set(this Fluent fluent, ISetExpression complex)
            => throw new NotImplementedException();

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
    }
}
