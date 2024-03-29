﻿// TODO: [bnaya 2022-10-23] get statistic back (check if it possible on Neo4J and abstract it for both REDIS & neo4j)
// TODO: [bnaya 2022-10-23] use lambda to attach relationships 

namespace Weknow.GraphDbClient.Abstraction;

// TODO: [bnaya 2022-12-13] GetAsync should work with expression
// Example: var result = await responseGet.GetAsync<string>(user.__.desc);

/// <summary>
/// Graph Database response abstraction
/// </summary>
public interface IGraphDBResponse
{
    /// <summary>
    /// Gets the first result set as T
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
    ValueTask<T> GetAsync<T>();

    /// <summary>
    /// Gets the first result set as T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory">The factory.</param>
    /// <returns>
    /// the first result
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    ValueTask<T> GetAsync<T>(Func<IGraphDBRecord, T> factory);

    /// <summary>
    /// Gets the first result set
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
    IAsyncEnumerable<T> GetRangeAsync<T>();

    /// <summary>
    /// Gets the first result set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory">The factory.</param>
    /// <returns>
    /// the first result
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person) RETURN p
    /// var person = results.Get<Person>();
    /// ]]></example>
    IAsyncEnumerable<T> GetRangeAsync<T>(Func<IGraphDBRecord, T> factory);


    /// <summary>
    /// Cast result set as T by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="property">The property.</param>
    /// <returns>
    /// Results by keys
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]></example>
    ValueTask<T> GetAsync<T>(string key, string? property = null);

    /// <summary>
    /// Gets result set by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="property">The property.</param>
    /// <returns>
    /// Results by keys
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p, c
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]></example>
    IAsyncEnumerable<T> GetRangeAsync<T>(string key, string? property = null);

    ValueTask<IGraphExecutionSummary> GetInfoAsync();
}
