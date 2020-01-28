// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Weknow
{
    /// <summary>
    /// Pattern factory
    /// </summary>
    public static class PatternFactory
    {
        #region R / Relation

        /// <summary>
        /// Gets the Pattern
        /// R and Relation are the same (it's only matter of naming flavor)
        /// </summary>
        public static IRelation R => new Pattern(FluentCypher.Default, string.Empty);
        /// <summary>
        /// Gets the Pattern
        /// R and Relation are the same (it's only matter of naming flavor)
        /// </summary>
        public static IRelation Relation => new Pattern(FluentCypher.Default, string.Empty);

        #endregion // R / Relation

        #region N / Node

        /// <summary>
        /// Create Nodes pattern representation.
        /// N and Node are the same (it's only matter of naming flavor)
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static Pattern N(
            string variable,
            IEnumerable<string> labels,
            params FluentCypher[] properties)
        {
            return Node(variable, labels, properties);
        }

        /// <summary>
        /// Create Nodes pattern representation.
        /// N and Node are the same (it's only matter of naming flavor)
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static Pattern Node(
            string variable,
            IEnumerable<string> labels,
            params FluentCypher[] properties)
        {
            return new Pattern(FluentCypher.Default, PatternType.Node, variable, labels, properties);
        }

        /// <summary>
        /// Create Nodes pattern representation.
        /// N and Node are the same (it's only matter of naming flavor)
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static Pattern N(
            string variable,
            string label,
            params FluentCypher[] properties)
        {
            return Node(variable, label, properties);
        }

        /// <summary>
        /// Create Nodes pattern representation.
        /// N and Node are the same (it's only matter of naming flavor)
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static Pattern Node(
            string variable,
            string label,
            params FluentCypher[] properties)
        {
            return new Pattern(FluentCypher.Default, PatternType.Node, variable, label.AsYield(), properties);
        }

        #endregion // _ // Node
    }
}
