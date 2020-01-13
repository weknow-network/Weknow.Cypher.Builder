// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: Properties variable
// TODO: params avoid empty (first parameter shouldn't be part of the array)
// TODO: Relationship Functions: https://neo4j.com/docs/cypher-manual/3.5/functions/scalar/
// TODO: Aggregating Functions: https://neo4j.com/docs/cypher-manual/3.5/functions/aggregating/
// TODO: INDEX : https://neo4j.com/docs/cypher-manual/3.5/schema/index/
// TODO: spatial: https://neo4j.com/docs/cypher-manual/3.5/functions/spatial/
// TODO: date: https://neo4j.com/docs/cypher-manual/3.5/functions/temporal/
// TODO: Duration Functions: https://neo4j.com/docs/cypher-refcard/current/
// TODO: CONSTRAINT: https://neo4j.com/docs/cypher-manual/3.5/schema/constraints/
// TODO: Mathematical Functions: https://neo4j.com/docs/cypher-manual/3.5/functions/
// TODO: String Functions: https://neo4j.com/docs/cypher-manual/3.5/functions/string/
// TODO: Lists: https://neo4j.com/docs/cypher-manual/3.5/syntax/lists/, 
//              https://neo4j.com/docs/cypher-manual/3.5/functions/list/
// TODO: STARTS WITH, ENDS WITH, CONTAINS, IN, CASE, Path

using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using static Weknow.Helpers.Helper;

// TODO:  Cache (one / multi line), break line strategy (same operator in a row)

namespace Weknow
{
    /// <summary>
    /// Fluent Cypher
    /// </summary>
    [DebuggerTypeProxy(typeof(FluentCypherDebugView))]
    public abstract class FluentCypher :
        IEnumerable<FluentCypher>
    {
        private static readonly ObjectPoolProvider _objectPoolProvider = new DefaultObjectPoolProvider();
        private static readonly ObjectPool<StringBuilder> _stringBuilderPool = _objectPoolProvider.CreateStringBuilderPool();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected readonly FluentCypher? _previous;
        protected internal readonly string _cypher = string.Empty;
        protected internal readonly string _cypherClose = string.Empty;
        protected internal readonly IEnumerable<FluentCypher> _children = Array.Empty<FluentCypher>();
        protected internal readonly string _childrenSeperator = SPACE;
        protected internal readonly CypherPhrase _phrase;
        protected internal IImmutableDictionary<CypherFormat, string> _cache = ImmutableDictionary<CypherFormat, string>.Empty;
        protected internal IImmutableList<string> _additionalLabels = ImmutableList<string>.Empty;
        private protected CypherNamingConvention _nodeConvention = CypherNamingConvention.Default;
        private protected CypherNamingConvention _relationConvention = CypherNamingConvention.Default;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypher" /> class from being created.
        /// </summary>
        private protected FluentCypher()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCypher" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The delegated.</param>
        /// <param name="childrenSeparator">The children separator  (space if empty).</param>
        /// <param name="additionalLabels">The additional labels.</param>
        /// <param name="nodeConvention">The node convention.</param>
        /// <param name="relationConvention">The node convention.</param>
        private protected FluentCypher(
            FluentCypher copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            IImmutableList<string>? additionalLabels = null,
            CypherNamingConvention? nodeConvention = null,
            CypherNamingConvention? relationConvention = null)
        {
            _previous = copyFrom;
            _cypher = cypher;
            _phrase = phrase;
            _cypherClose = cypherClose ?? string.Empty;
            _children = children ?? Array.Empty<FluentCypher>();
            _childrenSeperator = childrenSeparator ?? copyFrom?._childrenSeperator ?? SPACE;
            _additionalLabels = additionalLabels ?? copyFrom?._additionalLabels ?? ImmutableList<string>.Empty;
            _nodeConvention = nodeConvention ?? copyFrom._nodeConvention;
            _relationConvention = relationConvention ?? copyFrom._relationConvention;
        }

        #endregion // Ctor

        #region Cypher Operators

        #region Add

        /// <summary>
        /// Adds a statement (any valid cypher query).
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public abstract FluentCypher Add(string statement);

        #endregion // Add

        #region Match

        /// <summary>
        /// Create MATCH phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// MATCH (n)-->(m)
        /// MATCH (n {name: 'Alice'})-->(m)
        /// ]]></example>
        public abstract FluentCypher Match(string statement);

        #endregion // Match

        #region OptionalMatch

        /// <summary>
        /// Create OPTIONAL MATCH phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// OPTIONAL MATCH (n)-[r]->(m)
        /// ]]></example>
        public abstract FluentCypher OptionalMatch(string statement);

        #endregion // OptionalMatch

        #region Where

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// WHERE n.property <> $value
        /// ]]></example>
        public abstract FluentCypherWhereExpression Where(string statement);

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        public abstract FluentCypherWhereExpression Where(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        public abstract FluentCypherWhereExpression Where(string variable, IEnumerable<string> moreNames);

        /// <summary>
        /// Create WHERE phrase, generated by expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <param name="compareSign">The compare sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Where ((User user) => user.Id))
        /// Result with
        /// WHERE user.Id = $Id
        /// 
        /// Where ((User user) => user.Id), ">")
        /// Result with
        /// WHERE user.Id > $Id AND
        /// ]]></example>
        public abstract FluentCypherWhereExpression Where<T>(
                    Expression<Func<T, dynamic>> propExpression,
                    string compareSign = "=");

        #endregion // Where

        #region Create

        /// <summary>
        /// Create CREATE phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CREATE (n {name: $value}) // Create a node with the given properties.
        /// CREATE (n $map) // Create a node with the given properties.
        /// CREATE (n) SET n = properties // Create nodes with the given properties.
        /// CREATE (n)-[r:KNOWS]->(m) // Create a relationship with the given type and direction; bind a variable to it.
        /// CREATE (n)-[:LOVES {since: $value}]->(m) // Create a relationship with the given type, direction, and properties.
        /// ]]></example>
        public abstract FluentCypher Create(string statement);

        #endregion // Create
        

        #region Merge

        /// <summary>
        /// Create MERGE phrase.
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET 
        ///   n.counter = coalesce(n.counter, 0) + 1,
        ///   n.accessTime = timestamp()
        /// ------------------------------------------  
        /// MATCH (a:Person {name: $value1}),
        ///       (b:Person {name: $value2})
        /// MERGE(a)-[r: LOVES]->(b)
        /// ------------------------------------------
        /// MATCH (a:Person {name: $value1})
        /// MERGE (a)-[r: KNOWS]->(b:Person {name: $value3})
        /// ]]></example>
        public abstract FluentCypher Merge(string statement);

        #endregion // Merge

        #region OnCreate

        /// <summary>
        /// Compose ON CREATE phrase.
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// 
        ///   n.counter = coalesce(n.counter, 0) + 1,
        ///   n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnCreate();

        /// <summary>
        /// Compose ON CREATE phrase.
        /// Match a pattern or create it if it does not exist.
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnCreate(string statement);

        #endregion // OnCreate

        #region // OnCreateSet

        ///// <summary>
        ///// Compose ON CREATE SET phrase
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="propNames">The property names.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// Merge("(n:Person {name: $value})")
        /////     .OnCreateSet(new [] {"name", "id"}, "n")
        ///// Result in:
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.name = $name, n.id =$id
        ///// -----------------------------------------------
        ///// Merge("(n:Person {name: $value})")
        /////     .OnCreateSet(new [] {"name", "id"}, "n", "prefix")
        ///// Result in:
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.name = $prefix_name, n.id =$prefix_id
        ///// -----------------------------------------------
        ///// Merge("(n:Person {name: $value})")
        /////     .OnCreateSet(new [] {"name", "id"}, "n", "prefix", ".")
        ///// Result in:
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.name = $prefix.name, n.id =$prefix.id
        ///// ]]></example>
        //public abstract FluentCypher OnCreateSet(
        //    IEnumerable<string> propNames,
        //    string variable,
        //    string? parameterPrefix = null,
        //    string parameterSeparator = "_");

        ///// <summary>
        ///// Compose ON CREATE SET phrase
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="name">The name.</param>
        ///// <param name="moreNames">The more names.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// Merge("(n:Person {name: $value})")
        /////     .OnCreateSet("n", "name", "id")
        ///// Result in:
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.name = $name, n.id =$id
        ///// ]]></example>
        //public abstract FluentCypher OnCreateSet(string variable, string name, params string[] moreNames);

        ///// <summary>
        ///// Compose ON CREATE SET phrase from a type expression.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="propExpression">The property expression.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// Merge("(n:Person {name: $value})")
        /////      .OnCreateSet("n", new [] {"name", "id"})
        ///// Result in:
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.name = $name, n.id =$id
        ///// ]]></example>
        //public abstract FluentCypherSet<T> OnCreateSet<T>(Expression<Func<T, dynamic>> propExpression);

        ///// <summary>
        ///// Compose ON CREATE SET phrase by convention.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="variable">The variable.</param>
        ///// <param name="filter">The filter.</param>
        ///// <returns></returns>
        ///// <example><![CDATA[
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.created = timestamp()
        ///// ON MATCH SET
        ///// n.counter = coalesce(n.counter, 0) + 1,
        ///// n.accessTime = timestamp()
        ///// ]]></example>
        //public abstract FluentCypher OnCreateSetByConvention<T>(string variable, Func<string, bool> filter); 

        #endregion // OnCreateSet

        #region OnMatch

        /// <summary>
        /// Compose ON MATCH phrase
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// 
        ///   n.counter = coalesce(n.counter, 0) + 1,
        ///   n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnMatch();

        /// <summary>
        /// Compose ON MATCH phrase
        /// Match a pattern or create it if it does not exist.
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnMatch(string statement);

        /// <summary>
        /// Compose ON MATCH SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnMatchSet(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose ON MATCH SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnMatchSet(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Compose ON MATCH SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypherSet<T> OnMatchSet<T>(Expression<Func<T, dynamic>> propExpression);

        /// <summary>
        /// Compose ON MATCH SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public abstract FluentCypher OnMatchSetByConvention<T>(string variable, Func<string, bool> filter);

        #endregion // OnMatch

        #region Set

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SET n.property1 = $value1, n.property2 = $value2 // Update or create a property.
        /// SET n = $map // Update or create a property.
        /// SET n += $map // Add and update properties, while keeping existing ones.
        /// SET n:Person // Adds a label Person to a node.
        /// ]]></example>
        public abstract FluentCypher Set(string statement);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set("n", new [] { nameof(Foo.Name), nameof(Bar.Id)})
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// ]]></example>
        public abstract FluentCypher Set(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set("n", nameof(Foo.Name), nameof(Bar.Id))
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// ]]></example>
        public abstract FluentCypher Set(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set((User user) => user.Name)
        /// SET user.Name = $Name // Update or create a property.
        /// ]]></example>
        public abstract FluentCypherSet<T> Set<T>(Expression<Func<T, dynamic>> propExpression);

        #endregion // Set

        #region SetEntity

        /// <summary>
        /// Set instance. 
        /// Behaviors:
        /// Replace: This will remove any existing properties.
        /// Update: update properties, while keeping existing ones.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="paramName"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set("u", "entity")
        /// SET u = $u_entity
        /// ]]></example>
        public abstract FluentCypher SetEntity(
            string variable,
            string paramName = "",
            SetInstanceBehavior behavior = SetInstanceBehavior.Update);

        /// <summary>
        /// Set instance.
        /// Behaviors:
        /// Replace: This will remove any existing properties.
        /// Update: update properties, while keeping existing ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set<UserEntity>("u")
        /// SET u = $u_UserEntity
        /// ]]></example>
        public abstract FluentCypher SetEntity<T>(string variable, SetInstanceBehavior behavior = SetInstanceBehavior.Update);

        #endregion // SetEntity

        #region SetAll

        /// <summary>
        /// Set all properties (optional with excludes).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set<UserEntity>("u")
        /// SET u = $UserEntity
        /// ]]></example>
        public abstract FluentCypher SetAll<T>(string variable, params Expression<Func<T, dynamic>>[] excludes);

        #endregion // SetAll

        #region SetByConvention

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set((User user) =&gt; user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// ]]></example>
        public abstract FluentCypher SetByConvention<T>(string variable, Func<string, bool> filter);

        #endregion // SetByConvention

        #region SetLabel

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SET n:Person
        /// ]]></example>
        public abstract FluentCypher SetLabel(string variable, string label);

        #endregion // SetLabel

        #region Remove

        /// <summary>
        /// Create REMOVE phrase,
        /// Remove the label from the node or property.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// REMOVE n:Person // Remove a label from n.
        /// REMOVE n.property // Remove a property.
        /// ]]></example>
        public abstract FluentCypher Remove(string statement);

        #endregion // Remove

        #region Delete

        /// <summary>
        /// Create DELETE  phrase,
        /// Delete a node and a relationship.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (n)
        /// DETACH DELETE n
        /// ]]></example>
        public abstract FluentCypher Delete(string statement);

        #endregion // Delete

        #region DetachDelete

        /// <summary>
        /// Create DETACH DELETE phrase,
        /// Delete all nodes and relationships from the database.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (n)
        /// DETACH DELETE n
        /// ]]></example>
        public abstract FluentCypher DetachDelete(string statement);

        #endregion // DetachDelete

        #region Unwind 

        /// <summary>
        /// Create UNWIND phrase.
        /// With UNWIND, any list can be transformed back into individual rows. 
        /// The example matches all names from a list of names.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// UNWIND $names AS name
        /// MATCH(n { name: name})
        /// RETURN avg(n.age)
        /// ]]></example>
        public abstract FluentCypher Unwind(string collection, string variable);

        #endregion // Unwind

        #region ForEach

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// FOREACH (r IN relationships(path) | SET r.marked = true)
        /// ]]></example>
        public abstract FluentCypher ForEach(string statement);

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ForEach("n", "nations", nameof(Foo.Name), nameof(Bar.Id))
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// ]]></example>
        public abstract FluentCypher ForEach(string variable, string collection, params string[] propNames);

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ForEach("n", "nations", new [] {nameof(Foo.Name), nameof(Bar.Id)})
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// ]]></example>
        public abstract FluentCypher ForEach(string variable, string collection, IEnumerable<string> propNames);

        #endregion // ForEach

        #region ForEachByConvention

        /// <summary>
        /// Compose ForEach phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ForEach("$users", name =&gt; name.EndsWith("Name"))
        /// ForEach(user IN $users | SET user.FirstName = $user.FirstName, user.LastName = $user.LastName) // Update or create a property.
        /// ]]></example>
        public abstract FluentCypher ForEachByConvention<T>(string variable, string collection, Func<string, bool> filter);

        #endregion // ForEachByConvention

        #region With 

        /// <summary>
        /// Create WITH  phrase.
        /// WThe WITH syntax is similar to RETURN.
        /// It separates query parts explicitly,
        /// allowing you to declare which variables to carry over to the next part.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// WITH user, count(friend) AS friends
        /// WHERE friends > 10
        /// RETURN user
        /// ]]></example>
        public abstract FluentCypherReturn With(string statement);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN * // Return the value of all variables.
        /// RETURN n AS columnName // Use alias for result column name.
        /// ]]></example>
        public abstract FluentCypherReturn Return(string statement);

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <returns></returns>
        public abstract FluentCypherReturn Return();

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// .Return<Foo>(f => f.Name)
        /// RETURN f.Name
        /// ]]></example>
        public abstract FluentCypherReturn Return<T>(Expression<Func<T, dynamic>> expression);

        #endregion // Return

        #region ReturnDistinct 

        /// <summary>
        /// Create RETURN DISTINCT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN DISTINCT n // Return unique rows.
        /// ]]></example>
        public abstract FluentCypherReturn ReturnDistinct(string statement);

        /// <summary>
        /// Create RETURN DISTINCT phrase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// .ReturnDistinct<Foo>(f => f.Name)
        /// RETURN DISTINCT f.Name
        /// ]]></example>
        public abstract FluentCypherReturn ReturnDistinct<T>(Expression<Func<T, dynamic>> expression);

        #endregion // ReturnDistinct

        #region Union  

        /// <summary>
        /// Create UNION phrase.
        /// Returns the distinct union of all query results.
        /// Result column types and names have to match.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
        public abstract FluentCypher Union();

        #endregion // Union

        #region UnionAll  

        /// <summary>
        /// Create UNION ALL phrase.
        /// Returns the distinct union of all query results.
        /// Result column types and names have to match.
        /// Including duplicated rows.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION All
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
        public abstract FluentCypher UnionAll();

        #endregion // UnionAll

        #region Call

        /// <summary>
        /// Create CALL phrase,
        /// Delete a node and a relationship.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CALL db.labels() YIELD label
        /// This shows a standalone call to the built-in
        /// procedure db.labels to list all labels used in the database.
        /// Note that required procedure arguments are given explicitly
        /// in brackets after the procedure name.
        /// ]]></example>
        public abstract FluentCypher Call(string statement);

        #endregion // Call 

        #region Composite

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="childrenExpression">The delegate expression.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public abstract FluentCypher Composite(
            Func<FluentCypher, FluentCypher> childrenExpression,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null);

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="moreChildren">The more children.</param>
        /// <returns></returns>
        public abstract FluentCypher Composite(
            FluentCypher child,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null,
            string? childrenSeparator = null,
            params FluentCypher[] moreChildren);

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="moreChildren">The more children.</param>
        /// <returns></returns>
        public abstract FluentCypher Composite(
            FluentCypher child,
            string childrenSeparator,
            params FluentCypher[] moreChildren);

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="children">The delegated.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public abstract FluentCypher Composite(
            IEnumerable<FluentCypher> children,
            string? childrenSeparator = null,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null);

        #endregion // Composite

        #region As

        /// <summary>
        /// Create As phrase
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// collect(list) AS items
        /// /// ]]></example>
        public abstract FluentCypher As(string name);

        #endregion // As

        #endregion // Cypher Operators

        #region Entity

        /// <summary>
        /// Node mutation by entity.
        /// </summary>
        public abstract ICypherEntityMutations Entity { get; }

        #endregion // Entity

        #region Entities

        /// <summary>
        /// Nodes mutation by entities (use Unwind pattern).
        /// </summary>
        public abstract ICypherEntitiesMutations Entities { get; }

        #endregion // Entities

        #region Context

        /// <summary>
        /// Represent contextual label operations.
        /// Enable to add additional common labels like environment or tenants
        /// </summary>
        public abstract ICypherContext Context { get; }

        #endregion // Context

        #region ToCypher

        /// <summary>
        /// Gets the cypher statement.
        /// </summary>
        public string ToCypher(CypherFormat cypherFormat = CypherFormat.MultiLineDense) => GenerateCypher(cypherFormat);

        #endregion // ToCypher

        #region GenerateCypher

        /// <summary>
        /// Generates the cypher.
        /// </summary>
        /// <param name="cypherFormat">The cypher format.</param>
        /// <returns></returns>
        private string GenerateCypher(CypherFormat cypherFormat)
        {
            if (_cache.TryGetValue(cypherFormat, out string cypher))
                return cypher;

            IEnumerable<FluentCypher> forward = this;
            IEnumerable<FluentCypher> backward = ReverseEnumerable();
            StringBuilder sb = _stringBuilderPool.Get();
            try
            {
                sb = Aggregate(cypherFormat, forward, backward, sb);
                cypher = sb.ToString();
                _cache = _cache.Add(cypherFormat, cypher);
            }
            finally
            {
                sb.Clear();
                _stringBuilderPool.Return(sb);

            }

            return cypher;
        }

        #endregion // GenerateCypher

        #region Aggregate

        private static StringBuilder Aggregate(
            CypherFormat cypherFormat,
            IEnumerable<FluentCypher> forward,
            IEnumerable<FluentCypher> backward,
            StringBuilder sb)
        {
            sb = forward.Aggregate(sb, (acc, current) => AccumulateForward(acc, current, cypherFormat)); // tag or open tag
            sb = backward.Aggregate(sb, (acc, current) => AccumulateBackward(acc, current, cypherFormat)); // close tag
            return sb;
        }

        #endregion // Aggregate

        #region AccumulateForward

        /// <summary>
        /// Accumulates function for aggregation - used fromGenerateCypher .
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="current">The current.</param>
        /// <param name="cypherFormat">The cypher format.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Delegation must be of phrase None</exception>
        private static StringBuilder AccumulateForward(StringBuilder sb, FluentCypher current, CypherFormat cypherFormat)
        {

            int repeat = RepeatCount(current);
            sb = cypherFormat switch
            {
                CypherFormat.MultiLineDense => FormatMultiLineDense(current, sb, repeat)
                                                           .FormatStatement(current, repeat),
                CypherFormat.MultiLine => FormatMultiLine(current, sb)
                                                           .FormatStatement(current, repeat),
                _ => FormatSingleLine(current, sb).FormatStatement(current, repeat)
            };


            FluentCypher? firstChild = current._children.FirstOrDefault();
            if (firstChild != null)
            {
                IEnumerable<FluentCypher> backward = firstChild.ReverseEnumerable();
                sb = Aggregate(cypherFormat, firstChild, backward, sb);

                foreach (FluentCypher child in current._children.Skip(1))
                {
                    sb.Append(current._childrenSeperator);
                    //if (cypherFormat != CypherFormat.SingleLine && sb[sb.Length - 1] != Environment.NewLine.Last())
                    //    sb.Append(Environment.NewLine);
                    backward = child.ReverseEnumerable();
                    sb = Aggregate(cypherFormat, child, backward, sb);
                }
            }

            return sb;
        }

        #endregion // AccumulateForward

        #region AccumulateBackward

        /// <summary>
        /// Accumulates function for aggregation - used fromGenerateCypher .
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="current">The current.</param>
        /// <param name="cypherFormat">The cypher format.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Delegation must be of phrase None</exception>
        private static StringBuilder AccumulateBackward(StringBuilder sb, FluentCypher current, CypherFormat cypherFormat)
        {
            return sb.Append(current._cypherClose);
        }

        #endregion // AccumulateBackward

        #region FormatSingleLine

        /// <summary>
        /// Formats the single line.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="sb">The sb.</param>
        private static StringBuilder FormatSingleLine(FluentCypher current, StringBuilder sb)
        {
            if (sb.Length == 0)
                return sb;
            sb = FormatConnectionPhrase(current, sb);
            if (sb[sb.Length - 1] != SPACE[0])
                sb = sb.Append(SPACE);
            return sb;
        }

        #endregion // FormatSingleLine

        #region FormatMultiLine

        /// <summary>
        /// Formats the multi line.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="sb">The sb.</param>
        private static StringBuilder FormatMultiLine(FluentCypher current, StringBuilder sb)
        {
            if (sb.Length == 0)
                return sb;
            switch (current._phrase)
            {
                case CypherPhrase.Where:
                case CypherPhrase.Set:
                    if (sb[sb.Length - 1] != LINE_SEPERATOR[LINE_SEPERATOR.Length - 1])
                        sb = sb.Append(LINE_SEPERATOR);
                    sb = sb.Append(INDENT);
                    break;
                case CypherPhrase.OnMatch:
                case CypherPhrase.OnCreate:
                    if (sb[sb.Length - 1] != LINE_SEPERATOR[LINE_SEPERATOR.Length - 1])
                        sb = sb.Append(LINE_SEPERATOR);
                    sb = sb.Append(HALF_INDENT);
                    break;
                case CypherPhrase.And:
                case CypherPhrase.Or:
                case CypherPhrase.Count:
                    if (sb[sb.Length - 1] != SPACE[0])
                        sb = sb.Append(SPACE);
                    break;
                default:
                    if (sb[sb.Length - 1] != LINE_SEPERATOR[LINE_SEPERATOR.Length - 1])
                        sb = sb.Append(LINE_SEPERATOR);
                    break;
            }

            sb = FormatConnectionPhrase(current, sb);
            return sb;
        }

        #endregion // FormatMultiLine

        #region FormatMultiLineDense

        /// <summary>
        /// Formats the multi line dense.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="sb">The sb.</param>
        /// <param name="repeat">The repeat.</param>
        /// <returns></returns>
        private static StringBuilder FormatMultiLineDense(FluentCypher current, StringBuilder sb, int repeat)
        {
            if (sb.Length == 0)
                return sb;
            if (repeat == 0 || repeat % BREAK_LINE_ON == 0)
            {
                sb = FormatMultiLine(current, sb);
                return sb;
            }

            sb = FormatSingleLine(current, sb);
            return sb;
        }

        #endregion // FormatMultiLineDense

        #region FormatConnectionPhrase

        /// <summary>
        /// Formats the connection phrase.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="sb"></param>
        private static StringBuilder FormatConnectionPhrase(FluentCypher current, StringBuilder sb)
        {
            FluentCypher? previous = current._previous;
            if (previous == null)
                return sb;
            CypherPhrase curPhrase = current._phrase;
            CypherPhrase prevPhrase = previous._phrase;

            if (curPhrase == prevPhrase)
            {

                switch (previous._phrase)
                {
                    case CypherPhrase.Set:
                    case CypherPhrase.Return:
                    case CypherPhrase.With:
                    case CypherPhrase.ReturnDistinct:
                        sb = sb.Append(COMMA);
                        break;
                }
            }

            // TODO: handle CypherFactory methods like count, min, collect, etc...

            return sb;
        }

        #endregion // FormatConnectionPhrase

        #region RepeatCount

        /// <summary>
        /// Repeats the count.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        private static int RepeatCount(FluentCypher current)
        {
            CypherPhrase phrase = current._phrase;
            FluentCypher? previous = current._previous;
            int i = 0;
            while (previous != null && i < BREAK_LINE_ON)
            {
                CypherPhrase prevPhrase = previous._phrase;

                if (prevPhrase == CypherPhrase.Or || prevPhrase == CypherPhrase.And)
                {
                    //phrase = previous._phrase;
                    previous = previous._previous;
                    continue;
                }

                if (phrase != prevPhrase)
                    break;

                i++;

                phrase = previous._phrase;
                previous = previous._previous;
            }
            return i;
        }

        #endregion // RepeatCount

        #region Cast Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="CypherBuilder"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(FluentCypher builder)
        {
            return builder.ToCypher();
        }

        #endregion // Cast Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => ToCypher(CypherFormat.SingleLine);

        #endregion // ToString

        #region ReverseEnumerable

        /// <summary>
        /// Reverses the enumerable.
        /// </summary>
        /// <returns></returns>
        protected private IEnumerable<FluentCypher> ReverseEnumerable()
        {
            yield return this;
            if (_previous != null)
            {
                foreach (FluentCypher prev in _previous.ReverseEnumerable())
                {
                    yield return prev;
                }
            }
        }

        #endregion // ReverseEnumerable

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator<FluentCypher> IEnumerable<FluentCypher>.GetEnumerator()
        {
            if (_previous != null)
            {
                foreach (FluentCypher prev in _previous)
                {
                    yield return prev;
                }
            }
            yield return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<FluentCypher>)this).GetEnumerator();

        #endregion // IEnumerable

        #region FluentCypherDebugView

        private class FluentCypherDebugView
        {
            public FluentCypherDebugView(FluentCypher instance)
            {
                Cypher = instance.ToCypher(CypherFormat.MultiLineDense);
            }

            public string Cypher { get; }
        }

        #endregion // FluentCypherDebugView
    }
}
