using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// TODO: with concurrency increment

namespace Weknow
{
    /// <summary>
    /// Entity Mutations
    /// </summary>
    public interface ICypherEntityMutations
    {
        #region CreateNew

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="variable">The node's variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSeparator">The parameter separator.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("n", new [] {"A", "B"}, "map")
        /// Results in:
        /// CREATE (n:A:B $n_map)
        /// ----------------------------------------------------------
        /// CreateNew("n", new [] {"A", "B"}, "map", "prefix")
        /// Results in:
        /// CREATE (n:A:B $prefix_map)
        /// ----------------------------------------------------------
        /// CreateNew("n", new [] {"A", "B"}, "map", "prefix", ".")
        /// Results in:
        /// CREATE (n:A:B $prefix.map)
        /// ]]></example>
        FluentCypher CreateNew(
            string variable,
            IEnumerable<string> labels,
            string parameter,
            string? parameterPrefix = null,
            string parameterSeparator = "_");

        /// <summary>
        /// Create CREATE instance phrase
        /// </summary>
        /// <param name="variable">The node's variable.</param>
        /// <param name="label">The node's label which will be used for the parameter format (variable_label).</param>
        /// <param name="additionalLabels">Additional labels.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CREATE (n:LABEL $n_LABEL) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher CreateNew(string variable, string label, params string[] additionalLabels);

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <typeparam name="T">will be used as the node's label. this label will also use for the parameter format (variable_typeof(T).Name).</typeparam>
        /// <param name="variable">The node's variable.</param>
        /// <param name="parameter"></param>
        /// <param name="additionalLabels">Additional labels.</param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// CreateNew<Foo>("n")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher CreateNew<T>(
            string variable,
            string parameter,
            params string[] additionalLabels);

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <param name="additionalLabels">The additional labels.</param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[CreateNew<Foo>("n", CypherNamingConvention.CREAMING_CASE)
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", CypherNamingConvention.CREAMING_CASE, "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher CreateNew<T>(
            string variable,
            string parameter,
            CypherNamingConvention labelFormat,
            params string[] additionalLabels);

        #endregion // Create

        #region CreateIfNotExists

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default);


        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("p", "Person", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>("p", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">The match property expression.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>(p => p.name, "map")
        /// MERGE (p:Person {name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher CreateIfNotExists<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter);

        #endregion // CreateInstanceIfNew

        #region CreateOrUpdate

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="concurrencyField">When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency).</param>
        /// make sure to set unique constraint (on the matching properties), 
        /// otherwise a new node with different concurrency will be created when not match.
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map
        /// -------------------------------------------------------------------------
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// ]]></example>
        FluentCypher CreateOrUpdate(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default);

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("p", "Person", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map
        ///     
        /// CreateOrUpdate("p", "Person", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>("p", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map
        ///     
        /// CreateOrUpdate<Person>("p", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p += $map
        /// ]]></example>
        FluentCypher CreateOrUpdate<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or update.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">The match property expression.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="concurrencyField">The concurrency field.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>(p => p.name, "map")
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// ------------------------------------------------
        /// CreateOrUpdate<Person>(p => p.name, "map", "eTag")
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// ]]></example>
        FluentCypher CreateOrUpdate<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default);

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="concurrencyField">When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency).</param>
        /// make sure to set unique constraint (on the matching properties), 
        /// otherwise a new node with different concurrency will be created when not match.
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map
        /// -----------------------------------------------------------------------------
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map, p.eTag = p.eTag + 1
        /// ]]></example>
        FluentCypher CreateOrReplace(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default);

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace("p", "Person", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     ON CREATE SET p = $map
        ///     
        /// CreateOrReplace("p", "Person", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>("p", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map
        ///     
        /// CreateOrReplace<Person>("p", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p = $map
        /// ]]></example>
        FluentCypher CreateOrReplace<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or replace.
        /// For update use UpdateOrUpdate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">The match property expression.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="concurrencyField">The concurrency field.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>(p => p.name, "map")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map
        /// ---------------------------------------------------------
        /// CreateOrReplace<Person>(p => p.name, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map, p.eTag = p.eTag + 1
        /// ]]></example>
        FluentCypher CreateOrReplace<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default);

        #endregion // CreateOrReplace

        #region // UpdateIfExists

        ///// <summary>
        ///// Update entity if existing.
        ///// For replacing instance <seealso cref="ReplaceIfExists" />
        ///// </summary>
        ///// <param name="variable">The node variable.</param>
        ///// <param name="labels">The labels.</param>
        ///// <param name="entityParameter">The entity variable.</param>
        ///// <param name="matchProperties">The match properties.</param>
        ///// <param name="concurrencyField">The concurrency field.</param>
        ///// <param name="labelFormat">The label format.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// UpdateIfExists("p", "Person", "map", "name")
        ///// Results in:
        ///// Match (p:Person {name: $map.name})
        ///// SET p += $map
        ///// -------------------------------------------------------------------------
        ///// UpdateIfExists("p", "Person", "map", "name", "more", "eTag")
        ///// Results in:
        ///// Match (p:Person {name: $map.name, more: $map.more})
        ///// SET p += $map, p.eTag = p.eTag + 1
        ///// ]]></example>
        //FluentCypher UpdateIfExists(
        //    string variable,
        //    IEnumerable<string> labels,
        //    string entityParameter,
        //    IEnumerable<string> matchProperties,
        //    string? concurrencyField = null,
        //    CypherNamingConvention labelFormat = CypherNamingConvention.Default);

        ///// <summary>
        ///// Update entity if existing.
        ///// For replacing instance <seealso cref="ReplaceIfExists" />
        ///// </summary>
        ///// <param name="variable">The node variable.</param>
        ///// <param name="label">The label.</param>
        ///// <param name="entityParameter">The entity variable.</param>
        ///// <param name="matchProperty">The match property.</param>
        ///// <param name="moreMatchProperties">The more match properties.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// UpdateIfExists("p", "Person", "map", "name")
        ///// Match (p:Person {name: $map.name})
        /////     SET p += $map
        /////     
        ///// UpdateIfExists("p", "Person", "map", "name", "more")
        ///// Match (p:Person {name: $map.name, more: $map.more})
        /////     SET p += $map
        ///// ]]></example>
        //FluentCypher UpdateIfExists(
        //    string variable,
        //    string label,
        //    string entityParameter,
        //    string matchProperty,
        //    params string[] moreMatchProperties);

        ///// <summary>
        ///// Update entity if existing.
        ///// For replacing instance <seealso cref="ReplaceIfExists" />
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="variable">The node variable.</param>
        ///// <param name="entityParameter">The entity variable.</param>
        ///// <param name="matchProperty">The match property.</param>
        ///// <param name="moreMatchProperties">The more match properties.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// UpdateIfExists<Person></Person>("p", "map", "name")
        ///// Match (p:Person {name: $map.name})
        /////     SET p += $map
        /////     
        ///// UpdateIfExists<Person>("p", "map", "name", "more")
        ///// Match (p:Person {name: $map.name, more: $map.more})
        /////     SET p += $map
        ///// ]]></example>
        //FluentCypher UpdateIfExists<T>(
        //    string variable,
        //    string entityParameter,
        //    string matchProperty,
        //    params string[] moreMatchProperties);

        ///// <summary>
        ///// Update entity if existing.
        ///// For replacing instance <seealso cref="ReplaceIfExists"/>
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="matchProperty"></param>
        ///// <param name="entityParameter"></param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// UpdateIfExists<Person>(p => p.name, "map")
        ///// 
        ///// MERGE (p:Person {name: $map.name})
        /////     SET p = $map
        ///// ]]></example>
        //FluentCypher UpdateIfExists<T>(
        //    Expression<Func<T, dynamic>> matchPropertyExpression,
        //    string entityParameter);

        #endregion // UpdateIfExists
    }
}
