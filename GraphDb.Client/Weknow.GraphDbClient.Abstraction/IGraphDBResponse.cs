﻿// TODO: [bnaya 2022-10-23] get statistic back (check if it possible on Neo4J and abstract it for both REDIS & neo4j)
// TODO: [bnaya 2022-10-23] use lambda to attach relationships 

using Weknow.Mapping;

namespace Weknow.GraphDbClient.Abstraction;

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
    ValueTask<T[]> GetRangeAsync<T>();

    /// <summary>
    /// Cast result set as T by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="mappingKey">The mapping key use to project part of the result set as the result.</param>
    /// <returns>
    /// Results by keys
    /// </returns>
    /// <example><![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");    
    /// ]]></example>
    ValueTask<T> GetAsync<T>(string key, string? mappingKey = null);

    /// <summary>
    /// Gets result set by a key.
    /// </summary>
    /// <typeparam name="T">The type of the first result.</typeparam>
    /// <param name="key">The first result's key.</param>
    /// <param name="mappingKey">The mapping key use to project part of the result set as the result.</param>
    /// <returns>Results by keys</returns>
    /// <example>
    /// <![CDATA[
    /// MATCH (p:Person)-[:AT]->(c:Company) RETURN p, c
    /// 
    /// var person = results.Get<Person>("p");
    /// var company = results.Get<Company>("c");
    /// ]]>
    /// </example>
    ValueTask<T[]> GetRangeAsync<T>(string key, string? mappingKey = null);

    /// <summary>
    /// Gets result set by a key.
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
    ValueTask<(T1[], T2[])>  GetRangeAsync<T1, T2>(string key1, string key2);

    /// <summary>
    /// Gets result set by a key.
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
    ValueTask<(T1[], T2[], T3[])>  GetRangeAsync<T1, T2, T3>(string key1, string key2, string key3);         
}