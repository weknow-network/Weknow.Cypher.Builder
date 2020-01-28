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
    public class CypherText 
    {
        private readonly string _cypher;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherText"/> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        public CypherText(string cypher)
        {
            _cypher = cypher;
        }

        #endregion // Ctor

        #region ToCypher

        /// <summary>
        /// Converts to cypher.
        /// </summary>
        /// <param name="fluentCypher">The fluent cypher.</param>
        /// <returns></returns>
        public virtual string ToCypher(FluentCypher? fluentCypher) => _cypher;

        #endregion // ToCypher

        #region Cast Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="CypherBuilder" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CypherText cypher) => cypher.ToString();

        /// <summary>
        /// Performs an implicit conversion from <see cref="CypherBuilder" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CypherText(string cypher) => new CypherText(cypher);

        #endregion // Cast Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => _cypher;

        #endregion // ToString
    }
}