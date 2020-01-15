// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Weknow
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public abstract class FluentCypherWhereExpression : FluentCypherReturn
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private protected FluentCypherWhereExpression()
        {

        }

        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="config">The configuration.</param>
        private protected FluentCypherWhereExpression(CypherConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherWhereExpression" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        private protected FluentCypherWhereExpression(
            FluentCypher copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(copyFrom, cypher, phrase, cypherClose, 
                  children, childrenSeparator)
        {
        }

        #endregion // Ctor

        /// <summary>
        /// Compose AND phrase.
        /// </summary>
        /// <returns></returns>
        public abstract FluentCypher And { get; }
        /// <summary>
        /// Compose OR phrase.
        /// </summary>
        /// <returns></returns>
        public abstract FluentCypher Or { get; }
    }
}
