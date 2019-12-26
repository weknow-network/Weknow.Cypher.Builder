// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper


// https://neo4j.com/docs/cypher-manual/3.5/syntax/operators/

using System.Collections.Generic;

namespace Weknow
{

    public abstract class FluentCypherReturn: FluentCypher
    {
        #region Ctor

        private protected FluentCypherReturn()
        {

        }

        private protected FluentCypherReturn(
            FluentCypher? copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(copyFrom, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }

        #endregion // Ctor

        #region OrderBy 

        /// <summary>
        /// Create ORDER BY phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property
        /// </example>
        public abstract FluentCypherReturn OrderBy(string statement);

        #endregion // OrderBy

        #region OrderByDesc 

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property DESC
        /// </example>
        public abstract FluentCypherReturn OrderByDesc(string statement);

        #endregion // OrderByDesc

        #region Skip 

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP $skipNumber
        /// </example>
        public abstract FluentCypherReturn Skip(string statement);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP 10
        /// </example>
        public abstract FluentCypherReturn Skip(int number);

        #endregion // Skip

        #region Limit 

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT $skipNumber
        /// </example>
        public abstract FluentCypherReturn Limit(string statement);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT 10
        /// </example>
        public abstract FluentCypherReturn Limit(int number);

        #endregion // Limit

        #region Count 

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// RETURN count(*)
        /// </example>
        public abstract FluentCypherReturn Count();

        #endregion // Limit
    }
}
