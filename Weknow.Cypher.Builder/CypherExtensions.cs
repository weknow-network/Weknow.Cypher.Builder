using System;

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
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// </example>
        [Cypher("$0\r\nMATCH $1")]
        public static PD Match(this PD p, PD pp) => throw new NotImplementedException();

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
        public static PD Create(this PD p, PD pp) => throw new NotImplementedException();

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
        public static PD Merge(this PD p, PD pp) => throw new NotImplementedException();

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
        public static PD OnCreateSet(this PD p, IPropertiesOfType properties) => throw new NotImplementedException();

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
        public static PD OnCreateSet(this PD p, IVar var, IMap map) => throw new NotImplementedException();

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
        public static PD OnCreateSet(this PD p, IMap map) => throw new NotImplementedException();

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
        public static PD OnMatchSet(this PD p, IPropertiesOfType properties) => throw new NotImplementedException();

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
        [Cypher("$0\r\n\tON MATCH SET $1 = &$2")]
        public static PD OnMatchSet(this PD p, IVar var, IMap map) => throw new NotImplementedException();

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
        public static PD OnMatchSet(this PD p, IMap map) => throw new NotImplementedException();


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
        public static PD OptionalMatch(this PD p, PD pp) => throw new NotImplementedException();

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
        public static PD Where(this PD p, bool condition) => throw new NotImplementedException();

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
        public static PD Where(this PD p, IPropertiesOfType properties) => throw new NotImplementedException();

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
        [Cypher("$0\r\nRETURN $1")]
        public static PD Return(this PD p, params object[] vars) => throw new NotImplementedException();

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
        public static PD With(this PD p, params object[] vars) => throw new NotImplementedException();

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
        public static PD OrderBy(this PD p, params object[] vars) => throw new NotImplementedException();

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
        public static PD OrderByDesc(this PD p, params object[] vars) => throw new NotImplementedException();

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
        public static PD Skip(this PD p, int count) => throw new NotImplementedException();

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
        public static PD Limit(this PD p, int count) => throw new NotImplementedException();

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
        public static PD Set(this PD p, IVar node, ILabel label) => throw new NotImplementedException();

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
        public static PD Set(this PD p, IVar node, IVar map) => throw new NotImplementedException();

        /// <summary>
        /// SET phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="map">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// SET n.property1 = $value1,
        /// n.property2 = $value2
        /// </example>
        [Cypher("$0\r\n&SET $1 = &$1")]
        public static PD Set(this PD p, IMap map) => throw new NotImplementedException();

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
        public static PD Set(this PD p, IVar node) => throw new NotImplementedException();

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
        public static PD Set(this PD p, IPropertiesOfType properties) => throw new NotImplementedException();

        #endregion // Set
    }

}
