// TODO: [bnaya 2022-10-23] get statistic back (check if it possible on Neo4J and abstract it for both REDIS & neo4j)
// TODO: [bnaya 2022-10-23] use lambda to attach relationships 

namespace Weknow.GraphDbClient.Abstraction;

/// <summary>
/// Graph Database response abstraction
/// </summary>
public interface IGraphDBResponse
{
    /// <summary>
    /// Gets the first result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>the first result</returns>
    /// <example>
    /// <![CDATA[
    /// MATCH (p:Person) RETURN p
    /// 
    /// var person = results.Get<Person>();
    /// ]]>
    /// </example>
    T Get<T>();

    /// <summary>
    /// Gets result by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <returns>Results by keys</returns>
    /// <example>
    /// <![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p, c
    /// 
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]>
    /// </example>
    T Get<T>(string key);

    /// <summary>
    /// Gets result by a key.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="key1">The first result's key.</param>
    /// <param name="key2">The second result's key.</param>
    /// <returns>Results by keys</returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p, c
    /// var (person, company) = results.Get<Person>("p", "c");
    /// ]]></example>
    (T1, T2)  Get<T1, T2>(string key1, string key2);

    /// <summary>
    /// Gets result by a key.
    /// </summary>
    /// <typeparam name="T1">The type of the first result.</typeparam>
    /// <typeparam name="T2">The type of the second result.</typeparam>
    /// <typeparam name="T3">The type of the third result.</typeparam>
    /// <param name="key1">The first result's key.</param>
    /// <param name="key2">The second result's key.</param>
    /// <param name="key3">The third result's key.</param>
    /// <returns>Results by keys</returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[r:AT]->(c:Company) RETURN p, c, r
    /// var (person, company, at) = results.Get<Person>("p", "c", "r");
    /// ]]></example>
    (T1, T2, T3)  Get<T1, T2, T3>(string key1, string key2, string key3);         
}
