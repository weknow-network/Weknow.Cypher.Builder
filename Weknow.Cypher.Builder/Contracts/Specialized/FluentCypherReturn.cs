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
    public abstract class FluentCypherReturn: FluentCypher
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private protected FluentCypherReturn()
        {

        }


        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="config">The configuration.</param>
        private protected FluentCypherReturn(CypherConfig config)
            : base(config)
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
        private protected FluentCypherReturn(
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
        public abstract FluentCypherReturn OrderBy(string statement);

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
        public abstract FluentCypherReturn OrderByDesc(string statement);

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
        public abstract FluentCypherReturn Skip(string statement);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SKIP 10
        /// ]]></example>
        public abstract FluentCypherReturn Skip(int number);

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
        public abstract FluentCypherReturn Limit(string statement);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// LIMIT 10
        /// ]]></example>
        public abstract FluentCypherReturn Limit(int number);

        #endregion // Limit

        #region Count 

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN count(*)
        /// ]]></example>
        public abstract FluentCypherReturn Count();

        #endregion // Limit
    }
}
