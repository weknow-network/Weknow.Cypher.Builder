namespace Weknow
{
    /// <summary>
    /// Cypher Flavor
    /// </summary>
    public enum CypherFlavor
    {
        /// <summary>
        /// use compatible open cypher
        /// https://opencypher.org/
        /// </summary>
        OpenCypher,
        /// <summary>
        /// use neo4j 5 compatible cypher
        /// https://neo4j.com/docs/cypher-cheat-sheet/current/
        /// </summary>
        Neo4j5
    }
}
