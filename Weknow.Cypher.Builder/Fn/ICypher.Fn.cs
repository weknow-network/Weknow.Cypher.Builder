// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CypherBuilder;

/// <summary>
/// Entry point for constructing root level Cypher.
/// For fluent cypher check <see cref="CypherExtensions" />
/// </summary>
partial interface ICypher
{
    /// <summary>
    /// Gets the cypher's function.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public static IFn Fn => throw new NotImplementedException();

    public partial interface IFn
    {
    }
}