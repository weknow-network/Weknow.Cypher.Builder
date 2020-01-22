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
        /// Create New (throw if exists)
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The node's variable.
        /// When is null, it will be the first letter of the collection.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// CREATE (p:Person:Dev {id: map.id, name: map.name})
        /// SET p = map
        /// RETURN p
        /// -----------------------------------------------------------------
        /// CreateNew("items", new []{"Person", "Dev"}, new[] {"id", "name"})
        /// Results in:
        /// UNWIND items as item 
        /// CREATE (i:Person:Dev {id: item.id, name: item.name})
        /// SET i = map
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateNew(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable = null,
            string? item = null);


        /// <summary>
        /// Create New (throw if exists)
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("items", "Person", "id", "name")
        /// Results in:
        /// UNWIND items as item
        /// CREATE (p:Person {id: _item.id, name: _item.name})
        /// SET p = _item
        /// RETURN p
        /// ]]></example>
        FluentCypherReturnProjection CreateNew(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create New (throw if exists)
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the item when item is null.</param>
        /// <param name="item">The item (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// CREATE (p:Person {name: map.name})
        /// SET p = map
        /// RETURN p
        /// -----------------------------------------------
        /// CreateNew<Person>("items", p => p.name)
        /// Results in:
        /// UNWIND items as item 
        /// CREATE (p:Person {name: item.name})
        /// SET p = item
        /// RETURN p
        /// ]]></example>
        FluentCypherReturnProjection<T> CreateNew<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item = null);

        #endregion // CreateInstanceIfNew

        #region CreateIfNotExists

        /// <summary>
        /// Create if not exists
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The node's variable.
        /// When is null, it will be the first letter of the collection.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: map.id, name: map.name})
        /// ON CREATE SET p = map
        /// RETURN p
        /// ---------------------------------------------------------------
        /// CreateIfNotExists("items", new []{"Person", "Dev"}, new[] {"id", "name"})
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person:Dev {id: item.id, name: item.name})
        /// ON CREATE SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateIfNotExists(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable = null,
            string? item = null);


        /// <summary>
        /// Create if not exists
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("items", "Person", "id", "name")
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person {id: item.id, name: item.name})
        /// ON CREATE SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateIfNotExists(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Create if not exists       
        ///  
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the item when item is null.</param>
        /// <param name="item">The item (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: map.name})
        /// ON CREATE SET p = map
        /// RETURN p
        /// --------------------------------------------------------
        /// CreateIfNotExists<Person>("items", p => p.name)
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (p:Person {name: item.name})
        /// ON CREATE SET p = item
        /// RETURN p
        /// ]]></example>
        FluentCypherReturnProjection<T> CreateIfNotExists<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item = null);

        #endregion // CreateInstanceIfNew

        #region CreateOrUpdate

        /// <summary>
        /// Batch Create or update entities.
        /// For replace use ReplaceOrUpdate.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">Name of the collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The node's variable.
        /// When is null, it will be the first letter of the collection.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: map.id, name: map.name})
        /// SET p += map
        /// RETURN p
        /// -------------------------------------------------------------
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"})
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person:Dev {id: item.id, name: item.name})
        /// SET i += item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateOrUpdate(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable = null,
            string? item = null);

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("items", "Person", "name")
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person {name: item.name})
        /// SET i += item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateOrUpdate(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or update.
        /// For update use UpdateOrUpdate.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the item when item is null.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: map.name})
        /// SET p += map
        /// RETURN p
        /// -------------------------------------------
        /// CreateOrUpdate<Person>("items", p => p.name)
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person {name: item.name})
        /// SET i += item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection<T> CreateOrUpdate<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item = null);

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Creates the or replace.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="variable">The node's variable.
        /// When is null, it will be the first letter of the collection.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: map.id, name: map.name})
        /// SET p = map
        /// RETURN p
        /// ---------------------------------------------------------------------
        /// CreateOrUpdate("items", new []{"Person", "Dev"}, new[] {"id", "name"})
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person:Dev {id: item.id, name: item.name})
        /// SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateOrReplace(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable = null,
            string? item = null);

        /// <summary>
        /// Create or update entity.
        /// For update use UpdateOrUpdate.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace("items", "Person", "name")
        /// Results in:
        /// UNWIND items as item
        /// MERGE (i:Person {name: item.name})
        /// ON CREATE SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection CreateOrReplace(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties);

        /// <summary>
        /// Creates the or replace.
        /// For update use UpdateOrUpdate.
        /// 
        /// Make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="matchPropertyExpression">The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the item when item is null.</param>
        /// <param name="item">Will use singularized form of the collection</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>("items", p => p.name, "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: map.name})
        /// SET p = map
        /// RETURN p
        /// ------------------------------------------------------
        /// CreateOrReplace<Person>("items", p => p.name)
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person {name: item.name})
        /// SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection<T> CreateOrReplace<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item = null);

        #endregion // CreateOrReplace
    }
}
