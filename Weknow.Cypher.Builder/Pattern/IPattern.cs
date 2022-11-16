
namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Pattern primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The line and arrows in the following expression are patterns operations. 
    /// MATCH (n)-[r]->(m)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IPattern
    {
    }

}
