// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

namespace Weknow
{

    /// <summary>
    /// Extends the phrases option under WHERE context
    /// </summary>
    /// <seealso cref="Weknow.IFluentCypher" />
    public interface IFluentCypherExpression : IFluentCypher
    {
        /// <summary>
        /// Compose AND phrase.
        /// </summary>
        /// <returns></returns>
        IFluentCypher And();
        /// <summary>
        /// Compose OR phrase.
        /// </summary>
        /// <returns></returns>
        IFluentCypher Or();
    }
}
