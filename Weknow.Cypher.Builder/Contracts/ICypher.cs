// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

namespace Weknow
{
    public interface ICypher
    {
        /// <summary>
        /// Gets the cypher statement.
        /// </summary>
        string Cypher { get; }
        /// <summary>
        /// Gets the cypher statement trimmed into single line.
        /// </summary>
        string CypherLine { get; }
    }
}
