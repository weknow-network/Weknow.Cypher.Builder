// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;

namespace Weknow
{
    /// <summary>
    /// Relation contract
    /// </summary>
    public interface IRelation
    {
        /// <summary>
        /// Create Relations representation.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        Pattern this[
            string variable, 
            string type,
            params FluentCypher[] properties] { get; }
    }
}