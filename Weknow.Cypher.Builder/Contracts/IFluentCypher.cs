﻿// https://neo4j.com/docs/cypher-refcard/current/
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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weknow
{
    public interface IFluentCypher: 
        ICypherFluentWhere, 
        ICypherFluentForEach,
        ICypherable
    {
        #region Add

        /// <summary>
        /// Adds a statement (any valid cypher query).
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        IFluentCypher Add(string statement);

        #endregion // Add

        #region Match

        /// <summary>
        /// Create MATCH phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// MATCH (n)-->(m)
        /// MATCH (n {name: 'Alice'})-->(m)
        /// </example>
        IFluentCypher Match(string statement);

        #endregion // Match

        #region OptionalMatch

        /// <summary>
        /// Create OPTIONAL MATCH phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// OPTIONAL MATCH (n)-[r]->(m)
        /// </example>
        IFluentCypher OptionalMatch(string statement);

        #endregion // OptionalMatch

        #region Create

        /// <summary>
        /// Create CREATE phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// CREATE (n {name: $value}) // Create a node with the given properties.
        /// CREATE (n $map) // Create a node with the given properties.
        /// CREATE (n) SET n = properties // Create nodes with the given properties.
        /// CREATE (n)-[r:KNOWS]->(m) // Create a relationship with the given type and direction; bind a variable to it.
        /// CREATE (n)-[:LOVES {since: $value}]->(m) // Create a relationship with the given type, direction, and properties.
        /// </example>
        IFluentCypher Create(string statement);

        #endregion // Create

        #region Remove

        /// <summary>
        /// Create REMOVE phrase,
        /// Remove the label from the node or property.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        /// <example>
        /// REMOVE n:Person // Remove a label from n.
        /// REMOVE n.property // Remove a property.
        /// </example>
        IFluentCypher Remove(string statement);

        #endregion // Remove

        #region Delete

        /// <summary>
        /// Create DELETE  phrase,
        /// Delete a node and a relationship.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// DETACH DELETE n
        /// </example>
        IFluentCypher Delete(string statement);

        #endregion // Delete

        #region DetachDelete

        /// <summary>
        /// Create DETACH DELETE phrase,
        /// Delete all nodes and relationships from the database.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// DETACH DELETE n
        /// </example>
        IFluentCypher DetachDelete(string statement);

        #endregion // DetachDelete

        #region Merge

        /// <summary>
        /// Create MERGE phrase.
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
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
        /// </example>
        IFluentCypher Merge(string statement);

        #endregion // Merge

        #region OnCreate

        /// <summary>
        /// Compose ON CREATE phrase.
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// 
        ///   n.counter = coalesce(n.counter, 0) + 1,
        ///   n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnCreate();

        /// <summary>
        /// Compose ON CREATE phrase.
        /// Match a pattern or create it if it does not exist.
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnCreate(string statement);

        /// <summary>
        /// Compose ON CREATE SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnCreateSet(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose ON CREATE SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnCreateSet(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Compose ON CREATE SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypherSet<T> OnCreateSet<T>(Expression<Func<T, dynamic>> propExpression);

        /// <summary>
        /// Compose ON CREATE SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnCreateSetByConvention<T>(string variable, Func<string, bool> filter);

        #endregion // OnCreate

        #region OnMatch

        /// <summary>
        /// Compose ON MATCH phrase
        /// Match a pattern or create it if it does not exist. 
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// 
        ///   n.counter = coalesce(n.counter, 0) + 1,
        ///   n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnMatch();

        /// <summary>
        /// Compose ON MATCH phrase
        /// Match a pattern or create it if it does not exist.
        /// Use ON CREATE and ON MATCH for conditional updates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnMatch(string statement);

        /// <summary>
        /// Compose ON MATCH SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnMatchSet(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose ON MATCH SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnMatchSet(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Compose ON MATCH SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypherSet<T> OnMatchSet<T>(Expression<Func<T, dynamic>> propExpression);

        /// <summary>
        /// Compose ON MATCH SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// </example>
        IFluentCypher OnMatchSetByConvention<T>(string variable, Func<string, bool> filter);

        #endregion // OnMatch

        #region Set

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// SET n.property1 = $value1, n.property2 = $value2 // Update or create a property.
        /// SET n = $map // Update or create a property.
        /// SET n += $map // Add and update properties, while keeping existing ones.
        /// SET n:Person // Adds a label Person to a node.
        /// </example>
        IFluentCypher Set(string statement);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// Set("n", new [] { nameof(Foo.Name), nameof(Bar.Id)})
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// </example>
        IFluentCypher Set(string variable, IEnumerable<string> propNames);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example>
        /// Set("n", nameof(Foo.Name), nameof(Bar.Id))
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// </example>
        IFluentCypher Set(string variable, string name, params string[] moreNames);

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) => user.Name)
        /// SET user.Name = $Name // Update or create a property.
        /// </example>
        IFluentCypherSet<T> Set<T>(Expression<Func<T, dynamic>> propExpression);

        #endregion // Set

        #region SetReplaceInstance

        /// <summary>
        /// Set instance. This will remove any existing properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u = $UserEntity
        /// </example>
        IFluentCypher SetReplaceInstance<T>(string variable);

        #endregion // SetReplaceInstance

        #region SetUpdateInstance

        /// <summary>
        /// Add and update properties, while keeping existing ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u += $userEntity
        /// </example>
        IFluentCypher SetUpdateInstance<T>(string variable);

        #endregion // SetUpdateInstance

        #region SetAll

        /// <summary>
        /// Set all properties (optional with excludes).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u = $UserEntity
        /// </example>
        IFluentCypher SetAll<T>(string variable, params Expression<Func<T, dynamic>>[] excludes); 

        #endregion // SetAll

        #region SetByConvention

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// Set((User user) =&gt; user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// </example>
        IFluentCypher SetByConvention<T>(string variable, Func<string, bool> filter); 

        #endregion // SetByConvention

        #region SetLabel

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example>
        /// SET n:Person
        /// </example>
        IFluentCypher SetLabel<T>(string variable, string label); 

        #endregion // SetLabel

        #region Unwind 

        /// <summary>
        /// Create UNWIND phrase.
        /// With UNWIND, any list can be transformed back into individual rows. 
        /// The example matches all names from a list of names.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// UNWIND $names AS name
        /// MATCH(n { name: name})
        /// RETURN avg(n.age)
        /// </example>
        IFluentCypher Unwind(string collection, string variable);

        #endregion // Unwind

        #region With 

        /// <summary>
        /// Create WITH  phrase.
        /// WThe WITH syntax is similar to RETURN. 
        /// It separates query parts explicitly, 
        /// allowing you to declare which variables to carry over to the next part.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// WITH user, count(friend) AS friends
        /// WHERE friends > 10
        /// RETURN user
        /// </example>
        ICypherFluentReturn With(string statement);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// RETURN * // Return the value of all variables.
        /// RETURN n AS columnName // Use alias for result column name.
        /// </example>
        ICypherFluentReturn Return(string statement);

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        ICypherFluentReturn Return();

        #endregion // Return

        #region ReturnDistinct 

        /// <summary>
        /// Create RETURN DISTINCT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// RETURN DISTINCT n // Return unique rows.
        /// </example>
        ICypherFluentReturn ReturnDistinct(string statement);

        #endregion // ReturnDistinct

        #region Union  

        /// <summary>
        /// Create UNION phrase.
        /// Returns the distinct union of all query results.
        /// Result column types and names have to match.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// </example>
        IFluentCypher Union();

        #endregion // Union

        #region UnionAll  

        /// <summary>
        /// Create UNION ALL phrase.
        /// Returns the distinct union of all query results.
        /// Result column types and names have to match.
        /// Including duplicated rows.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION All
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// </example>
        IFluentCypher UnionAll();

        #endregion // UnionAll

        #region Call

        /// <summary>
        /// Create CALL phrase,
        /// Delete a node and a relationship.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        /// <example>
        /// CALL db.labels() YIELD label
        /// 
        /// This shows a standalone call to the built-in 
        /// procedure db.labels to list all labels used in the database.
        /// Note that required procedure arguments are given explicitly 
        /// in brackets after the procedure name.
        /// </example>
        IFluentCypher Call(string statement);

        #endregion // Call
    }
}
