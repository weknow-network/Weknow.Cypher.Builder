﻿// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

namespace Weknow.N4J
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.N4J.ICypherFluent" />
    public interface ICypherFluentWhereExpression : ICypherFluent
    {
        /// <summary>
        /// Compose AND phrase.
        /// </summary>
        /// <returns></returns>
        ICypherFluentWhere And();
        /// <summary>
        /// Compose OR phrase.
        /// </summary>
        /// <returns></returns>
        ICypherFluentWhere Or();
    }
}
