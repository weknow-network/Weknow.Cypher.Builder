// TODO: [bnaya 2022-10-23] get statistic back (check if it possible on Neo4J and abstract it for both REDIS & neo4j)
// TODO: [bnaya 2022-10-23] use lambda to attach relationships 

namespace Weknow.GraphDbClient.Abstraction;

public interface IGraphDBRecord
{
    /// <summary>
    /// Cast result as T by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="property">The property.</param>
    /// <returns>
    /// </returns>
    T Get<T>(string key, string? property = null);
}