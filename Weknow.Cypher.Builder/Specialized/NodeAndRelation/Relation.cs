// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using static Weknow.Helpers.Helper;

namespace Weknow
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class Relation : FluentCypher
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private Relation()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherWhereExpression" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        internal Relation(
            FluentCypher copyFrom,
            string variable,
            string type,
            ICypherPropertiesFactory? properties = null)
            : base(copyFrom, 
                  ComposeOpenCypher(variable, type, copyFrom.Config), 
                  CypherPhrase.Relation, 
                  "]",
                  (properties as FluentCypher)?.AsYield(), string.Empty, null)
        {
        }

        #endregion // Ctor

        #region ComposeOpenCypher

        /// <summary>
        /// Composes open cypher phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        private static string ComposeOpenCypher(
            string variable,
            string type,
            ICypherConfig config)
        {
            string formatted = config.Convention.FormatRelation(type);
            if (string.IsNullOrEmpty(variable))
                return $"[:{formatted}";
            return $"[{variable}:{formatted}";
        } 

        #endregion // ComposeOpenCypher
    }
}
