using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Cypher phrases for handling entities collection
    /// </summary>
    public interface ICypherEntitiesMutations
    {
        #region CreateNew

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("items", new [] {"A", "B"}, "n", "map")
        /// Results in:
        /// UNWIND items as map 
        /// CREATE (n:A:B map)
        /// ]]></example>
        FluentCypher CreateNew(
            string collection,
            IEnumerable<string> labels,
            string variable = "item",
            string? parameter = null);

        /// <summary>
        /// Create CREATE instance phrase
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The node's label which will be used for the parameter format (variable_label).</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("items", "FOO", "n")
        /// Results in:
        /// UNWIND items as n 
        /// CREATE (n:FOO $n) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew("items", "FOO", "n", "map")
        /// Results in:
        /// UNWIND items as map 
        /// CREATE (n:FOO:DEV $map) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher CreateNew(
            string collection,
            string label,
            string variable = "item",
            string? parameter = null);

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <typeparam name="T">will be used as the node's label. this label will also use for the parameter format (variable_typeof(T).Name).</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew<Foo>("items", "n")
        /// Results in:
        /// UNWIND items as n 
        /// CREATE (n:FOO n) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("items", "n", "map")
        /// Results in:
        /// UNWIND items as map 
        /// CREATE (n:FOO map) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher CreateNew<T>(
            string collection,
            string variable = "item",
            string? parameter = null);

        #endregion // Create

        #region CreateIfNotExists

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable = "item",
            string? parameter = null);


        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("items", "Person", "p", "map", "id", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>("items", "p", "map", "id", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter = null);

        #endregion // CreateInstanceIfNew

        #region CreateOrUpdate

        /// <summary>
        /// Batch Create or update entities.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <param name="collection">Name of the collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <returns></returns>
        /// make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// <example><![CDATA[
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable = "item",
            string? parameter = null);

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("items", "Person", "p", "map", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>("items", "p", "map", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or update.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter = null);

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Creates the or replace.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable = "item",
            string? parameter = null);

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace("items", "Person", "p", "map", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>("items", "p", "map", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or replace.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter = null);

        #endregion // CreateOrReplace
    }
}
