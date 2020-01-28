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
    /// Pattern representation
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    [DebuggerDisplay("{PatternType}: {_cypher}")]
    public class Pattern : FluentCypher, IRelation
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        protected private Pattern()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherWhereExpression" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        internal Pattern(
            FluentCypher copyFrom,
            string cypher)
            : base(copyFrom,
                  cypher,
                  CypherPhrase.Pattern)
        {
            PatternType = PatternType.Connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypherWhereExpression" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="patternType">Type of the pattern.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="tags">The labels.</param>
        /// <param name="properties">The properties.</param>
        internal Pattern(
            FluentCypher copyFrom,
            PatternType patternType,
            string variable,
            IEnumerable<string> tags,
            params FluentCypher[] properties)
            : base(copyFrom,
                  ComposeCypher(patternType, variable, tags),
                  CypherPhrase.Pattern,
                  patternType switch
                  {
                      PatternType.Node => ")",
                      PatternType.Relation => "]",
                      _ => string.Empty
                  },
                  properties, string.Empty, null)
        {
            PatternType = patternType;
        }

        #endregion // Ctor

        #region ComposeCypher

        /// <summary>
        /// Composes open cypher phrase.
        /// </summary>
        /// <param name="patternType">Type of the pattern.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="tags">The labels.</param>
        /// <returns></returns>
        private static LazyCypher ComposeCypher(
            PatternType patternType,
            string variable,
            IEnumerable<string> tags)
        {
            return new LazyCypher(c =>
            {
                var cfg = c.Configuration;
                switch (patternType)
                {
                    case PatternType.Node:
                        string finalLabels = cfg.AmbientLabels.Combine(tags);
                        if (string.IsNullOrEmpty(variable))
                            return $"(:{finalLabels}";
                        return $"({variable}:{finalLabels}";
                    case PatternType.Relation:
                        string? tag = tags.FirstOrDefault();
                        string formatted = (cfg as ICypherConfig).Convention.FormatRelation(tag);
                        if (string.IsNullOrEmpty(variable))
                            return $"[:{formatted}";
                        return $"[{variable}:{formatted}";
                    case PatternType.Connector:
                    default:
                        throw new NotImplementedException();
                }
            });
        }

        #endregion // ComposeCypher

        #region PatternType

        /// <summary>
        /// Gets the type of the pattern.
        /// </summary>
        internal PatternType PatternType { get; }

        #endregion // PatternType

        #region - operator overloads

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Pattern operator -(Pattern a, Pattern b)
        {
            FluentCypher combine = a.AddStatement("-", CypherPhrase.Pattern).Concat(b);
            return new Pattern(combine, string.Empty);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Pattern operator >(Pattern a, Pattern b)
        {
            FluentCypher combine = a.AddStatement("->", CypherPhrase.Pattern).Concat(b);
            return new Pattern(combine, string.Empty);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Pattern operator <(Pattern a, Pattern b)
        {
            FluentCypher combine = a.AddStatement("<-", CypherPhrase.Pattern).Concat(b);
            return new Pattern(combine, string.Empty);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Pattern operator *(Pattern a, Range b)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Pattern operator *(Pattern a, int b)
        {
            throw new NotImplementedException();
        }

        #endregion // - operator overloads

        #region Pattern this[...]

        /// <summary>
        /// Create Relations representation.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public Pattern this[
            string variable,
            string type,
            params FluentCypher[] properties]
        {
            get
            {
                return new Pattern(this, PatternType.Relation, variable, type.AsYield(), properties);
            }
        }

        #endregion // Pattern this[...]
    }
}
