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
    /// Lazy cypher 
    /// </summary>
    public class LazyCypher : CypherText
    {
        private readonly Func<FluentCypher, string> _composer;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyCypher" /> class.
        /// </summary>
        /// <param name="composer">The composer.</param>
        public LazyCypher(Func<FluentCypher, string> composer): base (string.Empty)
        {
            _composer = composer;
        }

        #endregion // Ctor

        #region ToCypher

        /// <summary>
        /// Converts to cypher.
        /// </summary>
        /// <param name="fluentCypher">The fluent cypher.</param>
        /// <returns></returns>
        public override string ToCypher(FluentCypher? fluentCypher) => 
            fluentCypher == null? string.Empty : _composer(fluentCypher);

        #endregion // ToCypher
    }
}