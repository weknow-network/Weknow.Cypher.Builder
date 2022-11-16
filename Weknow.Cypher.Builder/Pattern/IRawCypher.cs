
namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Pure cypher injection.
    /// Should used for non-supported cypher extensions
    /// </summary>
    [Obsolete("It's better to use the Cypher methods instead of clear text as log as it supported", false)]
    public interface IRawCypher : IPattern
    {
    }
}
