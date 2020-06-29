// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;

namespace Weknow
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class C : CypherBuilder
    {
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
            return new FluentCypher(configuration);
        }

        #endregion // static Create
    }
}