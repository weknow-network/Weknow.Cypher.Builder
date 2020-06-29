// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper


// https://neo4j.com/docs/cypher-manual/3.5/syntax/operators/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using static Weknow.Helpers.Helper;

namespace Weknow
{

    /// <summary>
    /// Fluent Cypher Return phrase
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class FluentCypherReturnProjection<T>: FluentCypherReturn
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression" /> class from being created.
        /// </summary>
        private protected FluentCypherReturnProjection()
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
        internal FluentCypherReturnProjection(
            FluentCypher copyFrom,
            string cypher = "",
            CypherPhrase phrase = CypherPhrase.None,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            CypherConfig? config = null)
            : base(copyFrom, cypher, phrase, cypherClose,
                  children, childrenSeparator, config)
        {
        }

        #endregion // Ctor

        #region Project

        /// <summary>
        /// Projects return properties.
        /// </summary>
        /// <param name="projectionExpression">The projection expression.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">The return variable expected to be in the previous phrase</exception>
        public FluentCypherReturnProjection<T> Project(
            Expression<Func<T, dynamic>> projectionExpression)
        {
            var (expVariable, property) = ExtractLambdaExpression(projectionExpression);

            #region Validation

            if (_previous == null)
            {
                throw new ArgumentNullException($"{nameof(_previous)} expected to be non null");
            }

            #endregion // Validation

            string prevCypher = _previous._cypher;
            int spaceIndex = prevCypher.LastIndexOf(" ");
            int dotIndex = prevCypher.LastIndexOf(".");
            #region Validation

            if (spaceIndex == -1)
            {
                if(dotIndex == -1)
                    throw new ArgumentOutOfRangeException($"The return variable expected to be in the previous phrase");
                spaceIndex = 0;
            }

            #endregion // Validation

            string variable = string.Empty;
            string prefix = string.Empty;
            if (dotIndex > spaceIndex)
            {
                prefix = ", ";
                variable = prevCypher.Substring(spaceIndex + 1, dotIndex - spaceIndex).Trim();
            }
            string statement = $"{prefix}{variable}.{property} ";
            return new FluentCypherReturnProjection<T>(this, statement, CypherPhrase.Dynamic);
        }

        #endregion // Project
    }

    /// <summary>
    /// Fluent Cypher Return phrase
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class FluentCypherReturnProjection: FluentCypherReturn
    {
        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypherWhereExpression"/> class from being created.
        /// </summary>
        private protected FluentCypherReturnProjection()
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
        internal FluentCypherReturnProjection(
            FluentCypher copyFrom,
            string cypher = "",
            CypherPhrase phrase = CypherPhrase.None,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            CypherConfig? config = null)
            : base(copyFrom, cypher, phrase, cypherClose,
                  children, childrenSeparator, config)
        {
        }

        #endregion // Ctor

        #region Project

        /// <summary>
        /// Projects return properties.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="moreProperties">The more properties.</param>
        /// <returns></returns>
        public FluentCypherReturn Project(
            string property, 
            params string[] moreProperties)
        {
            #region Validation

            if (_previous == null)
            {
                throw new ArgumentNullException($"{nameof(_previous)} expected to be non null");
            }

            #endregion // Validation

            string prevCypher = _previous._cypher;
            int spaceIndex = prevCypher.LastIndexOf(" ");
            int dotIndex = prevCypher.LastIndexOf(".");
            #region Validation

            if (dotIndex > spaceIndex)
            {
                throw new ArgumentOutOfRangeException($"The return variable expected to be without '.'");
            } 

            #endregion // Validation

            string variable = prevCypher.Substring(spaceIndex + 1).Trim();
            string statement = property;
            if (moreProperties != null && moreProperties.Length != 0)
            {
                var more = moreProperties.Select(p => $"{variable}.{p}");
                statement = $".{property}, {string.Join(", ", more)}";
            }
            return new FluentCypherReturn(_previous, statement, CypherPhrase.Project);
        }

        #endregion // Project
    }
}
