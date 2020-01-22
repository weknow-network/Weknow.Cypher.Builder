// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper


// https://neo4j.com/docs/cypher-manual/3.5/syntax/operators/

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Weknow
{

    /// <summary>
    /// Fluent Cypher Return phrase
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class FluentCypherReturn: FluentCypher
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private protected FluentCypherReturn()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherReturn" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        /// <param name="config">The configuration.</param>
        internal FluentCypherReturn(
            FluentCypher copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            CypherConfig? config = null)
            : base(copyFrom, cypher, phrase, cypherClose,
                  children, childrenSeparator, config)
        {
        }

        #endregion // Ctor

        #region OrderBy

        /// <summary>
        /// Create ORDER BY phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ORDER BY n.property
        /// ]]></example>
        public FluentCypherReturn OrderBy(string statement) =>
                            new FluentCypherReturn(this, statement, CypherPhrase.OrderBy);

        #endregion // OrderBy

        #region OrderByDesc

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ORDER BY n.property DESC
        /// ]]></example>
        public FluentCypherReturn OrderByDesc(string statement)
        {
            var result = new FluentCypherReturn(this, $"{statement} DESC", CypherPhrase.OrderByDesc);
            return result;
        }

        #endregion // OrderByDesc

        #region Skip

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SKIP $skipNumber
        /// ]]></example>
        public FluentCypherReturn Skip(string statement) =>
                            new FluentCypherReturn(this, statement, CypherPhrase.Skip);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SKIP 10
        /// ]]></example>
        public FluentCypherReturn Skip(int number) =>
                            new FluentCypherReturn(this, number.ToString(), CypherPhrase.Skip);

        #endregion // Skip

        #region Limit

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// LIMIT $skipNumber
        /// ]]></example>
        public FluentCypherReturn Limit(string statement) =>
                    new FluentCypherReturn(this, statement, CypherPhrase.Limit);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// LIMIT 10
        /// ]]></example>
        public FluentCypherReturn Limit(int number) =>
                            new FluentCypherReturn(this, number.ToString(), CypherPhrase.Limit);

        #endregion // Limit

        #region Count

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN count(*)
        /// ]]></example>
        public FluentCypherReturn Count() =>
                         new FluentCypherReturn(this, "(*)", CypherPhrase.Count);

        #endregion // Count
    }
}
