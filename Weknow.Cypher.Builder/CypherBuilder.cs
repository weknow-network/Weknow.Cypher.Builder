// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using static Weknow.Helpers.Helper;
using static Weknow.CypherFactory;
using System.Collections.Immutable;

namespace Weknow
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class C : CypherBuilder
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="C"/> class from being created.
        /// </summary>
        private protected C()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="C"/> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        protected internal C(string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="C"/> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        private protected C(FluentCypher copyFrom, string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(copyFrom, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }
    }

    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypherWhereExpression" />
    /// <seealso cref="Weknow.ICypherEntityMutations" />
    /// <seealso cref="Weknow.ICypherEntitiesMutations" />
    /// <seealso cref="Weknow.FluentCypher" />
    public class CypherBuilder :
        FluentCypher
    {
        #region static Create

        /// <summary>
        /// Root Cypher Builder with configuration.
        /// </summary>
        public static FluentCypher Create(Action<CypherConfig>? config = null)
        {
            var configuration = new CypherConfig();
            config?.Invoke(configuration);
            return new CypherBuilder(configuration);
        }

        #endregion // static Create

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypher" /> class from being created.
        /// </summary>
        internal CypherBuilder()
        {
        }

        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="config">The configuration.</param>
        private protected CypherBuilder(CypherConfig config)
            : base(config)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder" /> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        internal protected CypherBuilder(
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(Default, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        /// <param name="config">The configuration.</param>
        internal protected CypherBuilder(
            FluentCypher copyFrom,
            string cypher = "",
            CypherPhrase phrase = CypherPhrase.Dynamic,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            CypherConfig? config = null)
            : base(copyFrom, cypher, phrase, cypherClose,
                  children, childrenSeparator, config)
        {
        }

        #endregion // Ctor
    }
}