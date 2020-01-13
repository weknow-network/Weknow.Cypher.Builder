// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using static Weknow.Helpers.Helper;
using static Weknow.CypherFactory;

namespace Weknow
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class C : CypherBuilder
    {
        private protected C()
        {
        }

        protected internal C(string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }


        private protected C(FluentCypher? copyFrom, string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(copyFrom, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }
    }

    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class CypherBuilder :
        FluentCypherWhereExpression,
        ICypherAdvance,
        ICypherEntityMutations,
        ICypherEntitiesMutations
    {
        private protected static CypherNamingConvention _defaultNodeConvention = CypherNamingConvention.Default;
        private protected static CypherNamingConvention _defaultRelationConvention = CypherNamingConvention.Default;

        #region static Default

        /// <summary>
        /// Root Cypher Builder.
        /// </summary>
        public static readonly FluentCypher Default = new CypherBuilder();

        #endregion // static Default

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


        internal protected CypherBuilder(
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(null, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }

        private protected CypherBuilder(
            FluentCypher? copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(copyFrom, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }

        #endregion // Ctor

        #region IsWithCause

        /// <summary>
        /// Determines whether previous occurrence of phrase is cause for a WITH.  
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private bool IsWithCause(CypherPhrase phrase) => phrase switch
        {
            CypherPhrase.Merge => true,
            CypherPhrase.Create => true,
            _ => false
        };

        #endregion // IsWithCause

        #region IsWithCandidate

        /// <summary>
        /// Determines a phrase should cause checking previous phrases for cause of using WITH.  
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private bool IsWithCandidate(CypherPhrase phrase) => phrase switch
        {
            CypherPhrase.Match => true,
            CypherPhrase.Unwind => true,
            _ => false
        };

        #endregion // IsWithCandidate

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

            bool withCandidate = IsWithCandidate(phrase);

            #region bool hasPrevMerge = ...

            bool hasPrevMerge = false;
            if (withCandidate)
            {
                hasPrevMerge = this.ReverseEnumerable()
                .TakeWhile(m => m._phrase != CypherPhrase.With)
                .Any(m => IsWithCause(m._phrase));
            }

            #endregion // bool hasPrevMerge = ...

            if (withCandidate && hasPrevMerge)
                return new CypherBuilder((CypherBuilder)With("*"), statement, phrase);
            return new CypherBuilder(this, statement, phrase);
        }

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        private FluentCypherSet<T> AddStatement<T>(string statement, CypherPhrase phrase)
        {
            if (phrase == CypherPhrase.Dynamic || phrase == CypherPhrase.None)
                throw new NotImplementedException();

            return new FluentCypherSet<T>(this, statement, phrase);
        }

        #endregion // AddStatement

        #region Cypher Operators

        #region Generate

        /// <summary>
        /// Adds a statement (any valid cypher query).
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public static FluentCypher Generate(string statement) => Default.Add(statement);


        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="expression">The delegate expression.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public static FluentCypher Generate(
            Func<FluentCypher, FluentCypher> expression,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null)
        {
            return Default.Composite(expression, phrase, openCypher, closeCypher);
        }

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="moreChildren">The more children.</param>
        /// <returns></returns>
        public static FluentCypher Generate(
            FluentCypher child,
            string childrenSeparator,
            params FluentCypher[] moreChildren)
        {
            return Default.Composite(child, childrenSeparator, moreChildren);
        }

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="children">The delegated.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public static FluentCypher Generate(
            IEnumerable<FluentCypher> children,
            string? childrenSeparator = null,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null)
        {
            return Default.Composite(children, childrenSeparator, phrase, openCypher, closeCypher);
        }

        #endregion // Generate

        #region Add

        /// <summary>
        /// Adds a statement (any valid cypher query).
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public override FluentCypher Add(string statement) => AddStatement(statement, CypherPhrase.Dynamic);

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
        public override FluentCypher Match(string statement) => AddStatement(statement, CypherPhrase.Match);

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
        public override FluentCypher OptionalMatch(string statement) => AddStatement(statement, CypherPhrase.OptionalMatch);

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
        public override FluentCypher Create(string statement) => AddStatement(statement, CypherPhrase.Create);

        #endregion // Create

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
        public override FluentCypher Merge(string statement) => AddStatement(statement, CypherPhrase.Merge);

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
        public override FluentCypher OnCreate() => AddStatement(CypherPhrase.OnCreate);

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
        public override FluentCypher OnCreate(string statement) => AddStatement(statement, CypherPhrase.OnCreate);

        #endregion // OnCreate

        #region // OnCreateSet

        ///// <summary>
        ///// Compose ON CREATE SET phrase
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="propNames">The property names.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.created = timestamp()
        ///// ON MATCH SET
        ///// n.counter = coalesce(n.counter, 0) + 1,
        ///// n.accessTime = timestamp()
        ///// </example>
        //public override FluentCypher OnCreateSet(string variable, IEnumerable<string> propNames)
        //{
        //    #region Validation

        //    if (propNames == null || !propNames.Any())
        //        throw new ArgumentNullException($"{nameof(propNames)} must have at least single value");

        //    #endregion // Validation

        //    var root = AddStatement(CypherPhrase.OnCreate);
        //    return root.Set(variable, propNames);
        //}

        ///// <summary>
        ///// Compose ON CREATE SET phrase
        ///// </summary>
        ///// <param name="variable">The variable.</param>
        ///// <param name="propNames">The property names.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.created = timestamp()
        ///// ON MATCH SET
        ///// n.counter = coalesce(n.counter, 0) + 1,
        ///// n.accessTime = timestamp()
        ///// </example>
        //public override FluentCypher OnCreateSet(string variable, string name, params string[] moreNames)
        //{
        //    var root = AddStatement(CypherPhrase.OnCreate);
        //    return root.Set(variable, name.ToYield(moreNames));
        //}

        ///// <summary>
        ///// Compose ON CREATE SET phrase from a type expression.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="propExpression">The property expression.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.created = timestamp()
        ///// ON MATCH SET
        ///// n.counter = coalesce(n.counter, 0) + 1,
        ///// n.accessTime = timestamp()
        ///// </example>
        //public override FluentCypherSet<T> OnCreateSet<T>(Expression<Func<T, dynamic>> propExpression)
        //{
        //    var root = AddStatement(CypherPhrase.OnCreate);
        //    return root.Set<T>(propExpression);
        //}

        ///// <summary>
        ///// Compose ON CREATE SET phrase by convention.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="variable">The variable.</param>
        ///// <param name="filter">The filter.</param>
        ///// <returns></returns>
        ///// <example>
        ///// MERGE (n:Person {name: $value})
        ///// ON CREATE SET n.created = timestamp()
        ///// ON MATCH SET
        ///// n.counter = coalesce(n.counter, 0) + 1,
        ///// n.accessTime = timestamp()
        ///// </example>
        //public override FluentCypher OnCreateSetByConvention<T>(string variable, Func<string, bool> filter)
        //{
        //    var root = AddStatement(CypherPhrase.OnCreate);
        //    return root.SetByConvention<T>(variable, filter);
        // }

        #endregion // OnCreateSet

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
        public override FluentCypher OnMatch() => AddStatement(CypherPhrase.OnMatch);

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
        public override FluentCypher OnMatch(string statement) => AddStatement(statement, CypherPhrase.OnMatch);

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
        public override FluentCypher OnMatchSet(string variable, IEnumerable<string> propNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            FluentCypher set = root;
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
        public override FluentCypher OnMatchSet(string variable, string name, params string[] moreNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            FluentCypher set = root;
            return set.Set(variable, name.ToYield(moreNames));
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
        public override FluentCypherSet<T> OnMatchSet<T>(Expression<Func<T, dynamic>> propExpression)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            FluentCypher set = root;
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
        public override FluentCypher OnMatchSetByConvention<T>(string variable, Func<string, bool> filter)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            FluentCypher set = root;
            return set.SetByConvention<T>(variable, filter);
        }

        #endregion // OnMatch

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
        public override FluentCypher Remove(string statement) => AddStatement(statement, CypherPhrase.Remove);

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
        public override FluentCypher Delete(string statement) => AddStatement(statement, CypherPhrase.Delete);

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
        public override FluentCypher DetachDelete(string statement) => AddStatement(statement, CypherPhrase.DetachDelete);

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
        /// <example>
        /// UNWIND $names AS name
        /// MATCH(n { name: name})
        /// RETURN avg(n.age)
        /// </example>
        public override FluentCypher Unwind(string collection, string variable) => AddStatement($"{collection} AS {variable}", CypherPhrase.Unwind);

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
        public override FluentCypherReturn With(string statement) =>
                            AddStatement(statement, CypherPhrase.With);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public override FluentCypherReturn Return() =>
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
        public override FluentCypherReturn Return(string statement) =>
                            AddStatement(statement, CypherPhrase.Return);

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>
        /// .Return<Foo>(f => f.Name)
        /// RETURN f.Name
        /// </example>
        public override FluentCypherReturn Return<T>(Expression<Func<T, dynamic>> expression)
        {
            var (variable, name) = ExtractLambdaExpression(expression);
            return Return($"{variable}.{name}");
        }


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
        public override FluentCypherReturn ReturnDistinct(string statement) =>
                            AddStatement(statement, CypherPhrase.ReturnDistinct);

        /// <summary>
        /// Create RETURN DISTINCT phrase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <example>
        /// .ReturnDistinct<Foo>(f => f.Name)
        /// RETURN DISTINCT f.Name
        /// </example>
        public override FluentCypherReturn ReturnDistinct<T>(Expression<Func<T, dynamic>> expression)
        {
            var (variable, name) = ExtractLambdaExpression(expression);
            return ReturnDistinct($"{variable}.{name}");
        }

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
        public override FluentCypher Union() =>
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
        public override FluentCypher UnionAll() =>
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
        public override FluentCypher Call(string statement) =>
                            AddStatement(statement, CypherPhrase.Call);

        #endregion // Call

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
        public override FluentCypher Set(string statement) =>
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
        public override FluentCypher Set(string variable, IEnumerable<string> propNames)
        {
            CypherBuilder result = propNames.FormatSetWhere(variable)
                .Aggregate(this, (acc, name) => acc.AddStatement(name, CypherPhrase.Set));
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
        public override FluentCypher Set(string variable, string name, params string[] moreNames)
        {
            FluentCypher self = this;
            return self.Set(variable, name.ToYield(moreNames));
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
        public override FluentCypherSet<T> Set<T>(Expression<Func<T, dynamic>> propExpression)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            var result = AddStatement<T>($"{variable}.{name} = ${variable}_{name}", CypherPhrase.Set);
            return result;
        }

        #endregion // Set

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
        public override FluentCypher SetAll<T>(string variable, params Expression<Func<T, dynamic>>[] excludes)
        {
            IEnumerable<string> avoid = from exclude in excludes
                                        let lambda = ExtractLambdaExpression(exclude)
                                        select lambda.Name;
            var excludeMap = avoid.ToDictionary(m => m);

            FluentCypher self = this;
            var result =
                self.SetByConvention<T>(variable, name => !excludeMap.ContainsKey(name));
            return result;
        }

        #endregion // SetAll

        #region SetInstance

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
        /// <example>
        /// Set("u", "entity")
        /// SET u = $u_entity
        /// </example>
        public override FluentCypher SetEntity(
            string variable,
            string paramName = "",
            SetInstanceBehavior behavior = SetInstanceBehavior.Update)
        {
            string operand = behavior switch
            {
                SetInstanceBehavior.Replace => "=",
                _ => "+=",
            };
            string statement = string.IsNullOrEmpty(paramName) ? $"{variable} {operand} ${variable}" : $"{variable} {operand} ${variable}_{paramName}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        /// <summary>
        /// Set instance. 
        /// Behaviors:
        /// Replace: This will remove any existing properties.
        /// Update: update properties, while keeping existing ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns></returns>
        /// <example>
        /// Set<UserEntity>("u")
        /// SET u = $UserEntity
        /// </example>
        public override FluentCypher SetEntity<T>(
            string variable,
            SetInstanceBehavior behavior = SetInstanceBehavior.Update)
        {
            string operand = behavior switch
            {
                SetInstanceBehavior.Replace => "=",
                _ => "+=",
            };
            string statement = $"{variable} {operand} ${variable}_{typeof(T).Name}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        #endregion // SetInstance

        #region SetByConvention

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
        public override FluentCypher SetByConvention<T>(string variable, Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            FluentCypher self = this;
            var result = self.Set(variable, propNames);
            return result;
        }

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
        public override FluentCypher SetLabel(string variable, string label)
        {
            string statement = $"{variable}:{label}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        #endregion // SetLabel

        #region Where

        /// <summary>
        /// Create WHERE phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// WHERE n.property <> $value
        /// </example>
        public override FluentCypherWhereExpression Where(string statement) =>
                                            AddStatement(statement, CypherPhrase.Where);

        /// <summary>
        /// Create WHERE phrase with AND semantic between each term
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example>
        /// Where ("n", "A", "B")
        /// Result with
        /// WHERE n.A = $n_A AND n.B = $n_B
        /// </example>
        public override FluentCypherWhereExpression Where(
            string variable,
            string name,
            params string[] moreNames)
        {
            FluentCypher self = this;
            return self.Where(variable, name.ToYield(moreNames));
        }


        /// <summary>
        /// Create WHERE phrase with AND semantic between each term
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="names">The property names.</param>
        /// <returns></returns>
        /// <example>
        /// Where ("n", new [] {"A", "B"})
        /// Result with
        /// WHERE n.A = $n_A AND n.B = $n_B
        /// </example>
        public override FluentCypherWhereExpression Where(
            string variable,
            IEnumerable<string> names)
        {
            CypherBuilder result = names.FormatSetWhere(variable)
                    .Aggregate(this, (acc, name) =>
                            {
                                CypherBuilder b = acc;
                                if (acc != this)
                                {
                                    b = (CypherBuilder)acc.And;
                                }
                                b = b.AddStatement(name, CypherPhrase.Where);
                                return b;
                            });

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
        /// WHERE user.Id = $user_Id
        /// 
        /// Where ((User user) => user.Id), ">")
        /// Result with
        /// WHERE user.Id > $user_Id AND
        /// </example>
        public override FluentCypherWhereExpression Where<T>(
                    Expression<Func<T, dynamic>> propExpression,
                    string compareSign)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"{variable}.{name}";
            string prm = $"{variable}_{name}";
            statement = $"{statement} {compareSign} ${prm}";
            return AddStatement<T>(statement, CypherPhrase.Where);
        }


        #endregion // Where

        #region ForEach

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// FOREACH (r IN relationships(path) | SET r.marked = true)
        /// </example>
        public override FluentCypher ForEach(string statement) =>
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
        public override FluentCypher ForEach(
                        string variable,
                        string collection,
                        params string[] propNames)
        {
            FluentCypher self = this;
            FluentCypher result = self.ForEach(variable, collection, (IEnumerable<string>)propNames);
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
        public override FluentCypher ForEach(
                        string variable,
                        string collection,
                        IEnumerable<string> propNames)
        {
            IEnumerable<string> phrases = propNames.FormatSetWhere(variable);
            string sepStrategy = SetSeparatorStrategy(propNames);
            string sets = string.Join(sepStrategy, phrases);

            string sep = propNames.NewLineSeparatorStrategy();
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
        public override FluentCypher ForEachByConvention<T>(
                    string variable,
                    string collection,
                    Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            FluentCypher self = this;
            FluentCypher result = self.ForEach(variable, collection, propNames);
            return result;
        }

        #endregion // ForEach

        #region And

        /// <summary>
        /// Compose AND phrase.
        /// </summary>
        /// <returns></returns>
        public override FluentCypher And =>
            AddStatement(CypherPhrase.And);

        #endregion // And

        #region Or

        /// <summary>
        /// Compose OR phrase.
        /// </summary>
        /// <returns></returns>
        public override FluentCypher Or =>
            AddStatement(CypherPhrase.Or);

        #endregion // Or

        #region OrderBy

        /// <summary>
        /// Create ORDER BY phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property
        /// </example>
        public override FluentCypherReturn OrderBy(string statement) =>
                            AddStatement(statement, CypherPhrase.OrderBy);

        #endregion // OrderBy

        #region OrderByDesc

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// ORDER BY n.property DESC
        /// </example>
        public override FluentCypherReturn OrderByDesc(string statement)
        {
            var result = AddStatement($"{statement} DESC", CypherPhrase.OrderByDesc);
            return result;
        }

        #endregion // OrderByDesc

        #region Skip

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP $skipNumber
        /// </example>
        public override FluentCypherReturn Skip(string statement) =>
                            AddStatement(statement, CypherPhrase.Skip);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// SKIP 10
        /// </example>
        public override FluentCypherReturn Skip(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Skip);

        #endregion // Skip

        #region Limit

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT $skipNumber
        /// </example>
        public override FluentCypherReturn Limit(string statement) =>
                    AddStatement(statement, CypherPhrase.Limit);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example>
        /// LIMIT 10
        /// </example>
        public override FluentCypherReturn Limit(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Limit);

        #endregion // Limit

        #region As

        /// <summary>
        /// Create As phrase
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <example>
        /// collect(list) AS items
        /// /// </example>
        public override FluentCypher As(string name) => Add($"AS {name}");

        #endregion // As

        #region Count

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// RETURN count(*)
        /// </example>
        public override FluentCypherReturn Count() =>
                         AddStatement("(*)", CypherPhrase.Count);

        #endregion // Count

        #region Composite

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="expression">The delegate expression.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public override FluentCypher Composite(
            Func<FluentCypher, FluentCypher> expression,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null)
        {
            FluentCypher delegated = expression(Default);
            return Composite(delegated, phrase, openCypher, closeCypher);
        }

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="moreChildren">The more children.</param>
        /// <returns></returns>
        public override FluentCypher Composite(
            FluentCypher child,
            string childrenSeparator,
            params FluentCypher[] moreChildren)
        {
            return Composite(child, CypherPhrase.None, string.Empty, string.Empty, childrenSeparator, moreChildren);
        }

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
        public override FluentCypher Composite(
            FluentCypher child,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null,
            string? childrenSeparator = null,
            params FluentCypher[] moreChildren)
        {
            return Composite(child.ToYield(moreChildren), childrenSeparator, phrase, openCypher, closeCypher);
        }


        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="children">The delegated.</param>
        /// <param name="childrenSeparator">The children separator (space if empty).</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public override FluentCypher Composite(
            IEnumerable<FluentCypher> children,
            string? childrenSeparator = null,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null)
        {
            return new CypherBuilder(this, openCypher ?? string.Empty, phrase, closeCypher, children, childrenSeparator);
        }

        #endregion // Composite

        #endregion // Cypher Operators

        #region Advance

        /// <summary>
        /// Advance compositions.
        /// </summary>
        public override ICypherAdvance Advance => this;

        #endregion // Advance

        #region ICypherEntityMutations

        #region Entity

        /// <summary>
        /// Node mutation by entity.
        /// </summary>
        ICypherEntityMutations ICypherAdvance.Entity => this;

        #endregion // Entity

        #region CreateNew

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="variable">The node's variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <example>
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
        /// </example>
        FluentCypher ICypherEntityMutations.CreateNew(
            string variable,
            IEnumerable<string> labels,
            string parameter,
            string? parameterPrefix = null,
            string parameterSeparator = "_")
        {
            parameterPrefix = parameterPrefix ?? variable;
            string labelsStr = string.Join(":", labels);
            return AddStatement($"({variable}:{labelsStr} ${parameterPrefix}{parameterSeparator}{parameter})", CypherPhrase.Create);
        }

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="variable">The node's variable.</param>
        /// <param name="label">The node's label which will be used for the parameter format (variable_label).</param>
        /// <param name="additionalLabels">Additional labels.</param>
        /// <returns></returns>
        /// <example>
        /// CreateNew("n", "FOO")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew("n", "FOO", "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// </example>
        FluentCypher ICypherEntityMutations.CreateNew(string variable, string label, params string[] additionalLabels)
        {
            ICypherEntityMutations self = this;
            return self.CreateNew(variable, label.ToYield(additionalLabels), label);
        }


        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <typeparam name="T">will be used as the node's label. this label will also use for the parameter format (variable_typeof(T).Name).</typeparam>
        /// <param name="variable">The node's variable.</param>
        /// <param name="additionalLabels">Additional labels.</param>
        /// <returns></returns>
        /// <example>
        /// CreateNew<Foo>("n")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// </example>
        FluentCypher ICypherEntityMutations.CreateNew<T>(
            string variable,
            params string[] additionalLabels)
        {
            ICypherEntityMutations self = this;
            string label = typeof(T).Name;
            return self.CreateNew(variable, label.ToYield(additionalLabels), label);
        }

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <typeparam name="T">will be used as the node's label. this label will also use for the parameter format (variable_typeof(T).Name).</typeparam>
        /// <param name="variable">The node's variable.</param>
        /// <param name="labelFormat">The label formatting strategy.</param>
        /// <param name="additionalLabels">Additional labels.</param>
        /// <returns></returns>
        /// <example>
        /// CreateNew<Foo>("n", CypherNamingConvention.CREAMING_CASE)
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", CypherNamingConvention.CREAMING_CASE, "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// </example>
        FluentCypher ICypherEntityMutations.CreateNew<T>(
            string variable,
            CypherNamingConvention labelFormat,
            params string[] additionalLabels)
        {
            ICypherEntityMutations self = this;
            string label = typeof(T).Name;
            string formattedLabel = Format(label, labelFormat);
            return self.CreateNew(variable, formattedLabel.ToYield(additionalLabels), label);
        }

        #endregion // CreateNew

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
        /// <example>
        /// CreateIfNotExists("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default)
        {
            labels = labels.Select(n => Format(n));
            string joinedLabel = string.Format(":", labels);
            var props = P.Create(matchProperties, entityParameter, ".");
            return Merge($"({variable}:{joinedLabel} {props})")
                        .OnCreate()
                        .SetEntity(variable, entityParameter, SetInstanceBehavior.Replace);
        }


        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateIfNotExists("p", "Person", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateIfNotExists(variable, label.AsYield(), entityParameter, matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateIfNotExists<Person>("p", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateIfNotExists(variable, typeof(T).Name, entityParameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchPropertyExpression">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateIfNotExists<Person>(p => p.name, "map")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     ON CREATE SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntityMutations self = this;
            return self.CreateIfNotExists<T>(variable, entityParameter, matchProperty);
        }

        #endregion // CreateInstanceIfNew

        #region AddOrModify

        /// <summary>
        /// Add or Modify entity.
        /// For replace <seealso cref="ReplaceOrUpdate" />
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="concurrencyField">When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency)
        /// make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <param name="onMatchBehavior">The behavior.</param>
        /// <returns></returns>
        /// <example>
        /// AddOrModify("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag", SetInstanceBehavior.Update)
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// </example>
        private FluentCypher AddOrModify(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            string? concurrencyField,
            CypherNamingConvention labelFormat,
            SetInstanceBehavior onMatchBehavior)
        {
            bool withConcurrency = !string.IsNullOrEmpty(concurrencyField);
            string eTag = withConcurrency ? $"{variable}.{concurrencyField}" : string.Empty;

            labels = labels.Select(n => Format(n, labelFormat));
            string joinedLabel = string.Join(":", labels);
            var props = P.Create(matchProperties, entityParameter, ".");
            var result = Merge($"({variable}:{joinedLabel} {props})");

            if (withConcurrency)
            {
                result = result.Where($"{eTag} = ${entityParameter}.{concurrencyField}");
            }
            result = result.OnCreate()
                        .SetEntity(variable, entityParameter, SetInstanceBehavior.Replace);
            if (withConcurrency)
            {
                result = result.Add($", {eTag} = 0");
            }
            result = result.OnMatch()
                        .SetEntity(variable, entityParameter, onMatchBehavior);
            if (withConcurrency)
            {
                result = result.Add($", {eTag} = {eTag} + 1");
            }
            return result;
        }

        #endregion // AddOrModify

        #region CreateOrUpdate

        /// <summary>
        /// Create or update entity.
        /// For replace <seealso cref="ReplaceOrUpdate" />
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="concurrencyField">When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency)
        /// make sure to set unique constraint (on the matching properties), 
        /// otherwise a new node with different concurrency will be created when not match.
        /// </param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map
        /// -------------------------------------------------------------------------
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            string? concurrencyField,
            CypherNamingConvention labelFormat)
        {
            return AddOrModify(variable, labels, entityParameter, matchProperties, 
                                concurrencyField, labelFormat, SetInstanceBehavior.Update);
        }

        /// <summary>
        /// Create or update entity.
        /// For replace <seealso cref="ReplaceOrUpdate" />
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrUpdate("p", "Person", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map
        ///     
        /// CreateOrUpdate("p", "Person", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p += $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrUpdate(variable, label.AsYield(), entityParameter, matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Create or update entity.
        /// For replace <seealso cref="ReplaceOrUpdate" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrUpdate<Person>("p", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map
        ///     
        /// CreateOrUpdate<Person>("p", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p += $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrUpdate(variable, typeof(T).Name, entityParameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Creates the or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">The match property expression.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="concurrencyField">The concurrency field.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrUpdate<Person>(p => p.name, "map")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map
        /// ------------------------------------------------
        /// CreateOrUpdate<Person>(p => p.name, "map", "eTag")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p += $map, p.eTag = p.eTag + 1
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter,
            string? concurrencyField,
            CypherNamingConvention labelFormat)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            return AddOrModify(variable, typeof(T).Name.AsYield(), entityParameter, matchProperty.AsYield(), 
                concurrencyField, labelFormat, SetInstanceBehavior.Update);
        }

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Create or update entity.
        /// For update <seealso cref="UpdateOrUpdate" />
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
        /// <example>
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map
        /// -----------------------------------------------------------------------------
        /// CreateOrUpdate("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map, p.eTag = p.eTag + 1
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrReplace(
            string variable,
            IEnumerable<string> labels,
            string entityParameter,
            IEnumerable<string> matchProperties,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default)
        {
            return AddOrModify(variable, labels, entityParameter, matchProperties, 
                                concurrencyField, labelFormat, SetInstanceBehavior.Replace);
        }

        /// <summary>
        /// Create or update entity.
        /// For update <seealso cref="UpdateOrUpdate"/>
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrReplace("p", "Person", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     ON CREATE SET p = $map
        ///     
        /// CreateOrReplace("p", "Person", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrReplace(
            string variable,
            string label,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrReplace(variable, label.AsYield(), entityParameter, matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Create or update entity.
        /// For update <seealso cref="UpdateOrUpdate"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrReplace<Person>("p", "map", "name")
        /// 
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map
        ///     
        /// CreateOrReplace<Person>("p", "map", "name", "more")
        /// 
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        ///     SET p = $map
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrReplace<T>(
            string variable,
            string entityParameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrReplace(variable, typeof(T).Name, entityParameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Creates the or replace.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="entityParameter">The entity parameter.</param>
        /// <param name="concurrencyField">The concurrency field.</param>
        /// <param name="labelFormat">The label format.</param>
        /// <returns></returns>
        /// <example>
        /// CreateOrReplace<Person>(p => p.name, "map")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map
        /// ---------------------------------------------------------
        /// CreateOrReplace<Person>(p => p.name, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        ///     SET p = $map, p.eTag = p.eTag + 1
        /// </example>
        FluentCypher ICypherEntityMutations.CreateOrReplace<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string entityParameter,
            string? concurrencyField = null,
            CypherNamingConvention labelFormat = CypherNamingConvention.Default)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            return AddOrModify(variable, typeof(T).Name.AsYield(), entityParameter, matchProperty.AsYield(), 
                                concurrencyField, labelFormat, SetInstanceBehavior.Replace);
        }

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
        ///// <example>
        ///// UpdateIfExists("p", "Person", "map", "name")
        ///// Results in:
        ///// Match (p:Person {name: $map.name})
        ///// SET p += $map
        ///// -------------------------------------------------------------------------
        ///// UpdateIfExists("p", "Person", "map", "name", "more", "eTag")
        ///// Results in:
        ///// Match (p:Person {name: $map.name, more: $map.more})
        ///// SET p += $map, p.eTag = p.eTag + 1
        ///// </example>
        //FluentCypher ICypherEntityMutations.UpdateIfExists(
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
        ///// <example>
        ///// UpdateIfExists("p", "Person", "map", "name")
        ///// Match (p:Person {name: $map.name})
        /////     SET p += $map
        /////     
        ///// UpdateIfExists("p", "Person", "map", "name", "more")
        ///// Match (p:Person {name: $map.name, more: $map.more})
        /////     SET p += $map
        ///// </example>
        //FluentCypher ICypherEntityMutations.UpdateIfExists(
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
        ///// <example>
        ///// UpdateIfExists<Person></Person>("p", "map", "name")
        ///// Match (p:Person {name: $map.name})
        /////     SET p += $map
        /////     
        ///// UpdateIfExists<Person>("p", "map", "name", "more")
        ///// Match (p:Person {name: $map.name, more: $map.more})
        /////     SET p += $map
        ///// </example>
        //FluentCypher ICypherEntityMutations.UpdateIfExists<T>(
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
        ///// <example>
        ///// UpdateIfExists<Person>(p => p.name, "map")
        ///// 
        ///// MERGE (p:Person {name: $map.name})
        /////     SET p = $map
        ///// </example>
        //FluentCypher ICypherEntityMutations.UpdateIfExists<T>(
        //    Expression<Func<T, dynamic>> matchPropertyExpression,
        //    string entityParameter);

        #endregion // UpdateIfExists

        #endregion // ICypherEntityMutations


        #region Entities

        /// <summary>
        /// Nodes mutation by entities (use Unwind pattern).
        /// </summary>
        ICypherEntitiesMutations ICypherAdvance.Entities => this;

        #endregion // Entities

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
    }
}