// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;

namespace Weknow
{
    [DebuggerDisplay("{_cypherCommand.Cypher}")]
    internal class CypherBuilder<T> : CypherBuilder, ICypherFluentSet<T>
    {
        internal static readonly CypherBuilder<T> Empty = new CypherBuilder<T>();

        #region Ctor

        public CypherBuilder()
        {
        }

        public CypherBuilder(CypherBuilder copyFrom, string cypher, CypherPhrase phrase)
            : base(copyFrom, cypher, phrase)
        {
        }

        #endregion // Ctor

        #region SetMore

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <param name="propExpression">The property expression.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// Set((User user) =&gt; user.Name).Also(user =&gt; user.Id)
        /// SET user.Name = $user.Name, user.Id = $user.Id // Update or create a property.
        /// </example>
        ICypherFluentSet<T> ICypherFluentSet<T>.SetMore(Expression<Func<T, object>> propExpression)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"   ,{variable}.{name} = ${variable}_{name}";
            var result =  new CypherBuilder<T>(this, statement, CypherPhrase.Set);
            return result;
        }

        #endregion // SetMore
    }

    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.IFluentCypher" />
    [DebuggerDisplay("{CypherLine}")]
    public class CypherBuilder :
        IFluentCypher,
        ICypherFluentSetPlus,
        ICypherFluentReturn,
        ICypherFluentWhereExpression,
        ICypherable
    {
        private protected static CypherNamingConvention _defaultNodeConvention = CypherNamingConvention.Default;
        private protected static CypherNamingConvention _defaultRelationConvention = CypherNamingConvention.Default;

        private protected readonly CypherCommand _cypherCommand = CypherCommand.Empty;

        #region static Default

        /// <summary>
        /// Root Cypher Builder.
        /// </summary>
        public static readonly IFluentCypher Default = new CypherBuilder(); 

        #endregion // static Default

        #region ICypherable

        /// <summary>
        /// Gets the cypher statement.
        /// </summary>
        string ICypherable.Cypher => _cypherCommand.Cypher;

        /// <summary>
        /// Gets the cypher statement trimmed into single line.
        /// </summary>
        string ICypherable.CypherLine => _cypherCommand.CypherLine; 

        #endregion // ICypherable

        #region SetDefaultConventions

        /// <summary>
        /// Sets the default conventions.
        /// </summary>
        /// <param name="nodeConvention">The node convention.</param>
        /// <param name="relationConvention">The relation convention.</param>
        public static void SetDefaultConventions(
          CypherNamingConvention nodeConvention,
          CypherNamingConvention relationConvention)
        {
            _defaultNodeConvention = nodeConvention;
            _defaultRelationConvention = relationConvention;
        }

        #endregion // SetDefaultConventions

        #region Ctor

        private protected CypherBuilder()
        {
        }


        private protected CypherBuilder(
            CypherBuilder copyFrom,
            string cypher,
            CypherPhrase phrase)
        {
            _cypherCommand = new CypherCommand(
                                        copyFrom._cypherCommand,
                                        cypher,
                                        phrase);
        }

        private protected CypherBuilder(
            CypherBuilder copyFrom,
            CypherCommand cypherCommand)
        {
            _cypherCommand = cypherCommand;
        }

        #endregion // Ctor

        #region AddStatement

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private CypherBuilder AddStatement(CypherPhrase phrase) => AddStatement(string.Empty, phrase);

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private CypherBuilder AddStatement(string statement, CypherPhrase phrase)
        {
            if (phrase == CypherPhrase.Dynamic || phrase == CypherPhrase.None)
                return new CypherBuilder(this, statement, phrase);

            string prefix = GetPrefix(phrase);
            return new CypherBuilder(this, $"{prefix} {statement}", phrase);
        }

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private CypherBuilder<T> AddStatement<T>(string statement, CypherPhrase phrase)
        {
            if (phrase == CypherPhrase.Dynamic || phrase == CypherPhrase.None)
                throw new NotImplementedException();

            string prefix = GetPrefix(phrase);
            return new CypherBuilder<T>(this, $"{prefix} {statement}", phrase);
        }

        #endregion // AddStatement

        #region GetPrefix

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private static string GetPrefix(CypherPhrase phrase)
        {
            string prefix = string.Empty;
            switch (phrase)
            {
                case CypherPhrase.OptionalMatch:
                case CypherPhrase.DetachDelete:
                case CypherPhrase.UnionAll:
                case CypherPhrase.OnCreate:
                case CypherPhrase.OnMatch:
                case CypherPhrase.OrderBy:
                case CypherPhrase.ReturnDistinct:
                    prefix = phrase.ToString().ToSCREAMING(' ');
                    break;
                case CypherPhrase.OrderByDesc:
                    prefix = CypherPhrase.OrderBy.ToString().ToSCREAMING(' ');
                    break;
                case CypherPhrase.Count:
                    prefix = CypherPhrase.Count.ToString().ToLower();
                    break;
                default:
                    prefix = phrase.ToString().ToUpper();
                    break;
            }

            return prefix;
        }

        #endregion // GetPrefix

        #region ICypherFluent

        #region Add

        /// <summary>
        /// Adds a statement (any valid cypher query).
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        IFluentCypher IFluentCypher.Add(string statement) => AddStatement(statement, CypherPhrase.Dynamic);

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
        IFluentCypher IFluentCypher.Match(string statement) => AddStatement(statement, CypherPhrase.Match);

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
        IFluentCypher IFluentCypher.OptionalMatch(string statement) => AddStatement(statement, CypherPhrase.OptionalMatch);

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
        ICypherFluentSetPlus IFluentCypher.Create(string statement) => AddStatement(statement, CypherPhrase.Create);

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
        ICypherFluentSetPlus IFluentCypher.Remove(string statement) => AddStatement(statement, CypherPhrase.Remove);

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
        ICypherFluentSetPlus IFluentCypher.Delete(string statement) => AddStatement(statement, CypherPhrase.Delete);

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
        ICypherFluentSetPlus IFluentCypher.DetachDelete(string statement) => AddStatement(statement, CypherPhrase.DetachDelete);

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
        ICypherFluentSetPlus IFluentCypher.Merge(string statement) => AddStatement(statement, CypherPhrase.Merge);

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
        ICypherFluentSetPlus IFluentCypher.OnCreate() => AddStatement(CypherPhrase.OnCreate);

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
        IFluentCypher IFluentCypher.OnCreate(string statement) => AddStatement(statement, CypherPhrase.OnCreate);

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
        IFluentCypher IFluentCypher.OnCreateSet(string variable, IEnumerable<string> propNames)
        {
            #region Validation

            if (propNames == null || !propNames.Any())
                throw new ArgumentNullException($"{nameof(propNames)} must have at least single value");

            #endregion // Validation

            var root = AddStatement(CypherPhrase.OnCreate);
            ICypherFluentSet set = root;
            return set.Set(variable, propNames);
        }

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
        IFluentCypher IFluentCypher.OnCreateSet(string variable, string name, params string[] moreNames)
        {
            var root = AddStatement(CypherPhrase.OnCreate);
            ICypherFluentSet set = root;
            return set.Set(variable, Yilder(name, moreNames));
        }

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
        ICypherFluentSet<T> IFluentCypher.OnCreateSet<T>(Expression<Func<T, dynamic>> propExpression)
        {
            var root = AddStatement(CypherPhrase.OnCreate);
            ICypherFluentSet set = root;
            return set.Set<T>(propExpression);
        }

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
        IFluentCypher IFluentCypher.OnCreateSetByConvention<T>(string variable, Func<string, bool> filter)
        {
            var root = AddStatement(CypherPhrase.OnCreate);
            ICypherFluentSet set = root;
            return set.SetByConvention<T>(variable, filter);
        }

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
        ICypherFluentSetPlus IFluentCypher.OnMatch() => AddStatement(CypherPhrase.OnMatch);

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
        IFluentCypher IFluentCypher.OnMatch(string statement) => AddStatement(statement, CypherPhrase.OnMatch);

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
        IFluentCypher IFluentCypher.OnMatchSet(string variable, IEnumerable<string> propNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            ICypherFluentSet set = root;
            return set.Set(variable, propNames);
        }

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
        IFluentCypher IFluentCypher.OnMatchSet(string variable, string name, params string[] moreNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            ICypherFluentSet set = root;
            return set.Set(variable, Yilder(name, moreNames));
        }

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
        ICypherFluentSet<T> IFluentCypher.OnMatchSet<T>(Expression<Func<T, dynamic>> propExpression)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            ICypherFluentSet set = root;
            return set.Set<T>(propExpression);
        }

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
        IFluentCypher IFluentCypher.OnMatchSetByConvention<T>(string variable, Func<string, bool> filter)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            ICypherFluentSet set = root;
            return set.SetByConvention<T>(variable, filter);
        }

        #endregion // OnMatch

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
        IFluentCypher IFluentCypher.Unwind(string collection, string variable) => AddStatement($"{collection} AS {variable}", CypherPhrase.Unwind);

        #endregion // Unwind

        #region With 

        /// <summary>
        /// Create WITH  phrase.
        /// WThe WITH syntax is similar to RETURN.
        /// It separates query parts explicitly,
        /// allowing you to declare which variables to carry over to the next part.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// WITH user, count(friend) AS friends
        /// WHERE friends &gt; 10
        /// RETURN user
        /// </example>
        ICypherFluentReturn IFluentCypher.With(string statement) =>
                            AddStatement(statement, CypherPhrase.With);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        ICypherFluentReturn IFluentCypher.Return() =>
                            AddStatement(CypherPhrase.Return);

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// RETURN * // Return the value of all variables.
        /// RETURN n AS columnName // Use alias for result column name.
        /// RETURN DISTINCT n // Return unique rows.
        /// </example>
        ICypherFluentReturn IFluentCypher.Return(string statement) =>
                            AddStatement(statement, CypherPhrase.Return);

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
        ICypherFluentReturn IFluentCypher.ReturnDistinct(string statement) =>
                            AddStatement(statement, CypherPhrase.ReturnDistinct);

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
        IFluentCypher IFluentCypher.Union() =>
                            AddStatement(string.Empty, CypherPhrase.Union);

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
        IFluentCypher IFluentCypher.UnionAll() =>
                            AddStatement(string.Empty, CypherPhrase.UnionAll);

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
        ICypherFluentSetPlus IFluentCypher.Call(string statement) =>
                            AddStatement(statement, CypherPhrase.Call);

        #endregion // Call

        #endregion // ICypherFluent

        #region ICypherFluentWhere

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// WHERE n.property <> $value
        /// </example>
        ICypherFluentWhereExpression ICypherFluentWhere.Where(string statement) =>
                                            AddStatement(statement, CypherPhrase.Where);

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        ICypherFluentWhereExpression ICypherFluentWhere.Where(
            string variable,
            string name,
            params string[] moreNames)
        {
            ICypherFluentWhere self = this;
            return self.Where(variable, name, (IEnumerable<string>)moreNames);
        }


        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        ICypherFluentWhereExpression ICypherFluentWhere.Where(
            string variable,
            string name,
            IEnumerable<string> moreNames)
        {
            string statement = ComposeSetWhere(variable, Yilder(name, moreNames));
            var result = AddStatement(statement, CypherPhrase.Where);
            return result;
        }

        /// <summary>
        /// Create WHERE phrase, generated by expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <param name="compareSign">The compare sign.</param>
        /// <returns></returns>
        /// <example>
        /// Where ((User user) => user.Id))
        /// Result with
        /// WHERE user.Id = $Id
        /// 
        /// Where ((User user) => user.Id), ">")
        /// Result with
        /// WHERE user.Id > $user.Id AND
        /// </example>
        ICypherFluentWhereExpression ICypherFluentWhere.Where<T>(
                    Expression<Func<T, dynamic>> propExpression,
                    string compareSign)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"{variable}.{name}";
            string prm = $"{variable}_{name}";
            statement = $"{statement} {compareSign} ${prm}";
            return AddStatement<T>(statement, CypherPhrase.Where);
        }

        #endregion // ICypherFluentWhere

        #region ICypherFluentForEach

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// FOREACH (r IN relationships(path) | SET r.marked = true)
        /// </example>
        IFluentCypher ICypherFluentForEach.ForEach(string statement) =>
                            AddStatement(statement, CypherPhrase.ForEach);

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// ForEach("n", "nations", nameof(Foo.Name), nameof(Bar.Id))
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// </example>
        IFluentCypher ICypherFluentForEach.ForEach(
                        string variable,
                        string collection,
                        params string[] propNames)
        {
            ICypherFluentForEach self = this;
            IFluentCypher result = self.ForEach(variable, collection, (IEnumerable<string>)propNames);
            return result;
        }

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// ForEach("n", "nations", new [] {nameof(Foo.Name), nameof(Bar.Id)})
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// </example>
        IFluentCypher ICypherFluentForEach.ForEach(
                        string variable,
                        string collection,
                        IEnumerable<string> propNames)
        {
            string sets = ComposeSetWhere(variable, propNames);
            string sep = NewLineSeparatorStrategy(propNames);
            string statement = $@"({variable} IN {collection} | 
    SET {sep}{sets})";
            return AddStatement(statement, CypherPhrase.ForEach);
        }

        /// <summary>
        /// Compose ForEach phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example>
        /// ForEach("$users", name =&gt; name.EndsWith("Name"))
        /// ForEach(user IN $users | SET user.FirstName = $user.FirstName, user.LastName = $user.LastName) // Update or create a property.
        /// </example>
        IFluentCypher ICypherFluentForEach.ForEachByConvention<T>(
                    string variable,
                    string collection,
                    Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            ICypherFluentForEach self = this;
            IFluentCypher result = self.ForEach(variable, collection, propNames);
            return result;
        }

        #endregion // ICypherFluentForEach

        #region ICypherFluentSet

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
        IFluentCypher ICypherFluentSet.Set(string statement) =>
                            AddStatement(statement, CypherPhrase.Set);

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
        IFluentCypher ICypherFluentSet.Set(string variable, IEnumerable<string> propNames)
        {
            string statement = ComposeSetWhere(variable, propNames);
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// Set("n", nameof(Foo.Name), nameof(Bar.Id))
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// </example>
        IFluentCypher ICypherFluentSet.Set(string variable, string name, params string[] moreNames)
        {
            ICypherFluentSet self = this;
            return self.Set(variable, Yilder(name, moreNames));
        }

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
        ICypherFluentSet<T> ICypherFluentSet.Set<T>(Expression<Func<T, dynamic>> propExpression)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            var result = AddStatement<T>($"{variable}.{name} = ${variable}_{name}", CypherPhrase.Set);
            return result;
        }

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example>
        /// Set((User user) =&gt; user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// </example>
        IFluentCypher ICypherFluentSet.SetByConvention<T>(string variable, Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            ICypherFluentSet self = this;
            var result = self.Set(variable, propNames);
            return result;
        }

        /// <summary>
        /// Set all properties. This will remove any existing properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u = $userEntity
        /// </example>
        IFluentCypher ICypherFluentSet.Set<T>(string variable)
        {
            string statement = $"{variable} = ${typeof(T).Name.ToCamelCase()}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

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
        IFluentCypher ICypherFluentSet.SetUpdate<T>(string variable)
        {
            string statement = $"{variable} += ${typeof(T).Name.ToCamelCase()}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example>
        /// SET n:Person
        /// </example>
        IFluentCypher ICypherFluentSet.SetLabel<T>(string variable, string label)
        {
            string statement = $"{variable}:{typeof(T).Name}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        #endregion // ICypherFluentSet

        #region ICypherFluentWhereExpression

        /// <summary>
        /// Compose AND phrase.
        /// </summary>
        /// <returns></returns>
        ICypherFluentWhere ICypherFluentWhereExpression.And() =>
            AddStatement(CypherPhrase.Add);

        /// <summary>
        /// Compose OR phrase.
        /// </summary>
        /// <returns></returns>
        ICypherFluentWhere ICypherFluentWhereExpression.Or() =>
            AddStatement(CypherPhrase.Or);

        #endregion // ICypherFluentWhereExpression

        #region ICypherFluentReturn

        /// <summary>
        /// Create ORDER BY phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property
        /// </example>
        ICypherFluentReturn ICypherFluentReturn.OrderBy(string statement) =>
                            AddStatement(statement, CypherPhrase.OrderBy);

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property DESC
        /// </example>
        ICypherFluentReturn ICypherFluentReturn.OrderByDesc(string statement)
        {
            var result = AddStatement($"{statement} DESC", CypherPhrase.OrderByDesc);
            return result;
        }

        ICypherFluentReturn ICypherFluentReturn.Skip(string statement) =>
                            AddStatement(statement, CypherPhrase.Skip);

        ICypherFluentReturn ICypherFluentReturn.Skip(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Skip);

        ICypherFluentReturn ICypherFluentReturn.Limit(string statement) =>
                            AddStatement(statement, CypherPhrase.Limit);

        ICypherFluentReturn ICypherFluentReturn.Limit(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Limit);

        ICypherFluentReturn ICypherFluentReturn.Count() =>
                            AddStatement("(*)", CypherPhrase.Count);

        #endregion // ICypherFluentReturn

        #region Format

        /// <summary>
        /// Formats the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private protected string Format(
            string text,
            CypherNamingConvention convention = CypherNamingConvention.Default)
        {
            if (convention == CypherNamingConvention.Default)
                convention = _defaultNodeConvention;
            return convention switch
            {
                CypherNamingConvention.SCREAMING_CASE => text.ToSCREAMING(),
                CypherNamingConvention.CamelCase => text.ToCamelCase(),
                CypherNamingConvention.pacalCase => text.ToCamelCase(),
                _ => text
            };
        }

        /// <summary>
        /// Formats the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private protected string Format<T>(
            T text,
            CypherNamingConvention convention = CypherNamingConvention.Default)
        {
            if (convention == CypherNamingConvention.Default)
                convention = _defaultNodeConvention;
            string statement = text?.ToString() ?? throw new ArgumentNullException(nameof(text));
            return convention switch
            {
                CypherNamingConvention.SCREAMING_CASE => statement.ToSCREAMING(),
                CypherNamingConvention.CamelCase => statement.ToCamelCase(),
                CypherNamingConvention.pacalCase => statement.ToCamelCase(),
                _ => statement
            };
        }

        #endregion // Format

        #region Cast Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="CypherBuilder"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CypherBuilder builder)
        {
            return builder.ToString();
        } 

        #endregion // Cast Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            ICypherable self = this;
            return self.Cypher;
        } 

        #endregion // ToString
    }
}