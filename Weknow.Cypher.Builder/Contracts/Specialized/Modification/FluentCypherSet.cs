// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using static Weknow.Helpers.Helper;

namespace Weknow
{
    [DebuggerDisplay("{_cypherCommand.Cypher}")]
    public class FluentCypherSet<T> : CypherBuilder
    {
        // internal static readonly FluentCypherSet<T> Empty = new FluentCypherSet<T>();

        #region Ctor

        public FluentCypherSet()
        {
        }

        public FluentCypherSet(CypherBuilder copyFrom, string cypher, CypherPhrase phrase)
            : base(copyFrom, cypher, phrase)
        {
        }

        #endregion // Ctor
    }
}
