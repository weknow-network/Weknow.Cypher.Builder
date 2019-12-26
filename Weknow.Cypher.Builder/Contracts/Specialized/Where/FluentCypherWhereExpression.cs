﻿// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System.Collections.Generic;

namespace Weknow
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public abstract class FluentCypherWhereExpression : FluentCypherReturn
    {
        #region Ctor

        private protected FluentCypherWhereExpression()
        {

        }

        private protected FluentCypherWhereExpression(
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
