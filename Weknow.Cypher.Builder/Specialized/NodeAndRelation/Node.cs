// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Weknow
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class Node : FluentCypher
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private Node()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherWhereExpression" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="properties">The properties.</param>
        internal Node(
            FluentCypher copyFrom,
            string variable,
            IEnumerable<string> labels,
            ICypherPropertiesFactory? properties = null)
            : base(copyFrom, 
                  ComposeOpenCypher(variable, labels, copyFrom.Config), 
                  CypherPhrase.Node, 
                  ")",
                  (properties as FluentCypher)?.AsYield(), string.Empty, null)
        {
        }

        #endregion // Ctor

        #region ComposeOpenCypher

        /// <summary>
        /// Composes open cypher phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        private static string ComposeOpenCypher(
            string variable,
            IEnumerable<string> labels,
            ICypherConfig config)
        {
            string finalLabels = config.AmbientLabels.Combine(labels);
            if (string.IsNullOrEmpty(variable))
                return $"(:{finalLabels}";
            return $"({variable}:{finalLabels}";
        } 

        #endregion // ComposeOpenCypher
    }
}
