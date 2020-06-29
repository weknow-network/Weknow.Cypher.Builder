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

using static Weknow.CypherFactory;
using static Weknow.Helpers.Helper;

namespace Weknow
{
    /// <summary>
    /// Fluent Cypher
    /// </summary>
    [DebuggerTypeProxy(typeof(FluentCypherDebugView))]
    public class FluentCypher :
        IEnumerable<FluentCypher>,
        ICypherEntityMutations,
        ICypherEntitiesMutations
    {
        #region static Default

        /// <summary>
        /// Root Cypher Builder.
        /// </summary>
        internal static readonly FluentCypher Default = new FluentCypher();

        #endregion // static Default

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypher" /> class from being created.
        /// </summary>
        private protected FluentCypher()
        {
            _config = new CypherConfig();
        }

        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="config">The configuration.</param>
        internal FluentCypher(CypherConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder" /> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        internal protected FluentCypher(
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : this(Default, cypher, phrase, cypherClose, children, childrenSeparator)
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
        /// <param name="config">The configuration.</param>
        private protected FluentCypher(
            FluentCypher copyFrom,
            string cypher = "",
            CypherPhrase phrase = CypherPhrase.None,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null,
            CypherConfig? config = null)
        {
            _previous = copyFrom;
            _config = copyFrom._config;
            _cypher = cypher;
            _phrase = phrase;
            _cypherClose = cypherClose ?? string.Empty;
            _children = children ?? Array.Empty<FluentCypher>();
            _childrenSeperator = childrenSeparator ?? copyFrom._childrenSeperator ?? SPACE;
            _config = config ?? copyFrom._config;
        }

        #endregion // Ctor

        #region AddStatement

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        protected FluentCypher AddStatement(CypherPhrase phrase) => AddStatement(string.Empty, phrase);

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="phrase">The phrase.</param>
        /// <returns></returns>
        protected FluentCypher AddStatement(string statement, CypherPhrase phrase)
        {
            if (phrase == CypherPhrase.Dynamic || phrase == CypherPhrase.None)
                return new FluentCypher(this, statement, phrase);

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
                return new FluentCypher(With("*"), statement, phrase);
            return new FluentCypher(this, statement, phrase);
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

        #region AddAmbientLabels

        /// <summary>
        /// Adds the ambient labels.
        /// additional ambient labels which will be added to cypher queries
        /// (when the expression is not hard-codded string).
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns></returns>
        public FluentCypher AddAmbientLabels(params string[] labels) =>
            new FluentCypher(this, config: _config.Clone(labels));

        #endregion // AddAmbientLabels

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
        public FluentCypher Add(string statement) => AddStatement(statement, CypherPhrase.Dynamic);

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
        public FluentCypher Match(string statement) => AddStatement(statement, CypherPhrase.Match);

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
        public FluentCypher OptionalMatch(string statement) => AddStatement(statement, CypherPhrase.OptionalMatch);

        #endregion // OptionalMatch

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
        public FluentCypher Create(string statement) => AddStatement(statement, CypherPhrase.Create);

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
        public FluentCypher Merge(string statement) => AddStatement(statement, CypherPhrase.Merge);

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
        public FluentCypher OnCreate() => AddStatement(CypherPhrase.OnCreate);

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
        public FluentCypher OnCreate(string statement) => AddStatement(statement, CypherPhrase.OnCreate);

        #endregion // OnCreate

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
        public FluentCypher OnMatch() => AddStatement(CypherPhrase.OnMatch);

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
        public FluentCypher OnMatch(string statement) => AddStatement(statement, CypherPhrase.OnMatch);

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
        public FluentCypher OnMatchSet(string variable, IEnumerable<string> propNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            return root.Set(variable, propNames);
        }

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
        public FluentCypher OnMatchSet(string variable, string name, params string[] moreNames)
        {
            var root = AddStatement(CypherPhrase.OnMatch);
            return root.Set(variable, name.ToYield(moreNames));
        }

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
        public FluentCypherSet<T> OnMatchSet<T>(Expression<Func<T, dynamic>> propExpression)
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
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
        public FluentCypher OnMatchSetByConvention<T>(string variable, Func<string, bool> filter)
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
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// REMOVE n:Person // Remove a label from n.
        /// REMOVE n.property // Remove a property.
        /// ]]></example>
        public FluentCypher Remove(string statement) => AddStatement(statement, CypherPhrase.Remove);

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
        public FluentCypher Delete(string statement) => AddStatement(statement, CypherPhrase.Delete);

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
        public FluentCypher DetachDelete(string statement) => AddStatement(statement, CypherPhrase.DetachDelete);

        #endregion // DetachDelete

        #region Unwind 

        /// <summary>
        /// Create UNWIND phrase.
        /// With UNWIND, any list can be transformed back into individual rows.
        /// The example matches all names from a list of names.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="variable">The variable.</param>
        /// <param name="collectionSign">The collection sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Unwind("names", "name")
        /// Results in:
        /// UNWIND $names AS name
        /// -------------------------
        /// Unwind("names", "name", string.Empty)
        /// Results in:
        /// UNWIND names AS name
        /// -------------------------
        /// ]]></example>
        public FluentCypher Unwind(string collection, string variable, string collectionSign = "$") =>
            AddStatement($"{collectionSign}{collection} AS {variable}", CypherPhrase.Unwind);

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
        /// <example><![CDATA[
        /// MATCH (user)-[:FRIEND]-(friend)
        /// WHERE user.name = $name
        /// WITH user, count(friend) AS friends
        /// WHERE friends &gt; 10
        /// RETURN user
        /// ]]></example>
        public FluentCypherReturn With(string statement) =>
                        new FluentCypherReturn(this, statement, CypherPhrase.With);
                            //AddStatement(statement, CypherPhrase.With);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <returns></returns>
        public FluentCypherReturn Return() =>
                            new FluentCypherReturn(this, string.Empty, CypherPhrase.Return);

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN * // Return the value of all variables.
        /// RETURN n AS columnName // Use alias for result column name.
        /// RETURN DISTINCT n // Return unique rows.
        /// ]]></example>
        public FluentCypherReturn Return(string statement) =>
                            new FluentCypherReturn(this, statement, CypherPhrase.Return);

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
        public FluentCypherReturn Return<T>(Expression<Func<T, dynamic>> expression)
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
        /// <example><![CDATA[
        /// RETURN DISTINCT n // Return unique rows.
        /// ]]></example>
        public FluentCypherReturn ReturnDistinct(string statement) =>
                            new FluentCypherReturn(this, statement, CypherPhrase.ReturnDistinct);

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
        public FluentCypherReturn ReturnDistinct<T>(Expression<Func<T, dynamic>> expression)
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
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
        public FluentCypher Union() =>
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
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION All
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
        public FluentCypher UnionAll() =>
                            AddStatement(string.Empty, CypherPhrase.UnionAll);

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
        public FluentCypher Call(string statement) =>
                            AddStatement(statement, CypherPhrase.Call);

        #endregion // Call

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
        public FluentCypher Set(string statement) =>
                            AddStatement(statement, CypherPhrase.Set);

        /// <summary>
        /// Compose SET phrase
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set("n", new [] { nameof(Foo.Name), nameof(Bar.Id)})
        /// SET n.Name = $Name, n.Id = $Id // Update or create a property.
        /// ]]></example>
        private FluentCypher Set(
            string variable,
            IEnumerable<string> propNames,
            string? parameterPrefix = null,
            string parameterSign = "$")
        {
            FluentCypher result = propNames.FormatSetWhere(variable, parameterPrefix, parameterSign)
                .Aggregate(this, (acc, name) => acc.AddStatement(name, CypherPhrase.Set));
            return result;
        }

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
        public FluentCypher Set(string variable, string name, params string[] moreNames)
        {
            return Set(variable, name.ToYield(moreNames));
        }

        /// <summary>
        /// Compose SET phrase from a type expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set((User user) => user.Name)
        /// SET user.Name = $Name // Update or create a property.
        /// ]]></example>
        public FluentCypherSet<T> Set<T>(
            Expression<Func<T, dynamic>> propExpression,
            string? parameterPrefix = null,
            string parameterSign = "$")
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            var result = AddStatement<T>($"{variable}.{name} = {parameterSign}{parameterPrefix}{name}", CypherPhrase.Set);
            return result;
        }

        #endregion // Set

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
        public FluentCypher SetAll<T>(string variable, params Expression<Func<T, dynamic>>[] excludes)
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

        #region SetEntity

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="parameterSign">The parameter sign ($ or nothing).</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public FluentCypher SetEntity(
            string variable,
            string paramName,
            string parameterSign,
            SetInstanceBehavior behavior)
        {
            string operand = behavior switch
            {
                SetInstanceBehavior.Replace => "=",
                _ => "+=",
            };
            string statement = $"{variable} {operand} {parameterSign}{paramName}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

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
        public FluentCypher SetEntity(
            string variable,
            string paramName,
            SetInstanceBehavior behavior = SetInstanceBehavior.Update)
        {
            return SetEntity(variable, paramName, "$", behavior);
        }

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
        /// SET u = $UserEntity
        /// ]]></example>
        public FluentCypher SetEntity<T>(
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

        /// <summary>
        /// Set instance.
        /// Behaviors:
        /// Replace: This will remove any existing properties.
        /// Update: update properties, while keeping existing ones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set<UserEntity>("u")
        /// SET u = $UserEntity
        /// ]]></example>
        public FluentCypher SetEntity<T>(
            string variable,
            string parameterPrefix,
            SetInstanceBehavior behavior = SetInstanceBehavior.Update)
        {
            string operand = behavior switch
            {
                SetInstanceBehavior.Replace => "=",
                _ => "+=",
            };
            string statement = string.IsNullOrEmpty(parameterPrefix) ?
                                        $"{variable} {operand} ${typeof(T).Name}" :
                                        $"{variable} {operand} ${parameterPrefix}_{typeof(T).Name}";
            var result = AddStatement(statement, CypherPhrase.Set);
            return result;
        }

        #endregion // SetEntity

        #region SetByConvention

        /// <summary>
        /// Compose SET phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <example><![CDATA[
        /// Set((User user) =&gt; user.Name.StartWith("Name"))
        /// SET user.FirstName = $FirstName, usr.LastName = $LastName // Update or create a property.
        /// ]]></example>
        public FluentCypher SetByConvention<T>(string variable, Func<string, bool> filter)
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
        /// <param name="variable">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SET n:Person
        /// ]]></example>
        public FluentCypher SetLabel(string variable, string label)
        {
            string statement = $"{variable}:{_config.AmbientLabels.Combine(label)}";
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
        /// <example><![CDATA[
        /// WHERE n.property <> $value
        /// ]]></example>
        public FluentCypherWhereExpression Where(string statement) =>
                                            new FluentCypherWhereExpression(this, statement, CypherPhrase.Where);

        /// <summary>
        /// Create WHERE phrase with AND semantic between each term
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="name">The name.</param>
        /// <param name="moreNames">The more names.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Where ("n", "A", "B")
        /// Result with
        /// WHERE n.A = $n_A AND n.B = $n_B
        /// ]]></example>
        public FluentCypherWhereExpression Where(
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
        /// <example><![CDATA[
        /// Where ("n", new [] {"A", "B"})
        /// Result with
        /// WHERE n.A = $n_A AND n.B = $n_B
        /// ]]></example>
        public FluentCypherWhereExpression Where(
            string variable,
            IEnumerable<string> names)
        {
            FluentCypher result = names.FormatSetWhere(variable)
                    .Aggregate(this, (acc, name) =>
                    {
                        FluentCypher b = acc;
                        if (acc != this)
                        {
                            b = acc.AddStatement(CypherPhrase.And);
                        }
                        b = b.AddStatement(name, CypherPhrase.Where);
                        return b;
                    });

            return new FluentCypherWhereExpression(result, string.Empty, CypherPhrase.None);
        }

        /// <summary>
        /// Create WHERE phrase, generated by expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propExpression">The property expression.</param>
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <param name="compareSign">The compare sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Where ((User user) => user.Id))
        /// Result with
        /// WHERE user.Id = $user_Id
        /// Where ((User user) => user.Id), ">")
        /// Result with
        /// WHERE user.Id > $user_Id AND
        /// ]]></example>
        public FluentCypherWhereExpression Where<T>(
                    Expression<Func<T, dynamic>> propExpression,
                    string? parameterPrefix = null,
                    string parameterSign = "$",
                    string compareSign = "=")
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"{variable}.{name}";
            statement = $"{statement} {compareSign} {parameterSign}{parameterPrefix}{name}";
            return new FluentCypherWhereExpression(this, statement, CypherPhrase.Where);
        }

        #endregion // Where

        #region ForEach

        /// <summary>
        /// Compose ForEach phrase
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// FOREACH (r IN relationships(path) | SET r.marked = true)
        /// ]]></example>
        public FluentCypher ForEach(string statement) =>
                            AddStatement(statement, CypherPhrase.ForEach);

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
        public FluentCypher ForEach(
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
        /// <example><![CDATA[
        /// ForEach("n", "nations", new [] {nameof(Foo.Name), nameof(Bar.Id)})
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// ]]></example>
        public FluentCypher ForEach(
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
        /// <example><![CDATA[
        /// ForEach("$users", name =&gt; name.EndsWith("Name"))
        /// ForEach(user IN $users | SET user.FirstName = $user.FirstName, user.LastName = $user.LastName) // Update or create a property.
        /// ]]></example>
        public FluentCypher ForEachByConvention<T>(
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

        #region As

        /// <summary>
        /// Create As phrase
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// collect(list) AS items
        /// /// ]]></example>
        public FluentCypher As(string name) => Add($"AS {name}");

        #endregion // As

        #region Composite

        /// <summary>
        /// Adds the fluent cypher.
        /// </summary>
        /// <param name="expression">The delegate expression.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="openCypher">The open cypher.</param>
        /// <param name="closeCypher">The close cypher.</param>
        /// <returns></returns>
        public FluentCypher Composite(
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
        public FluentCypher Composite(
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
        public FluentCypher Composite(
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
        public FluentCypher Composite(
            IEnumerable<FluentCypher> children,
            string? childrenSeparator = null,
            CypherPhrase phrase = CypherPhrase.None,
            string? openCypher = null,
            string? closeCypher = null)
        {
            return new FluentCypher(this, openCypher ?? string.Empty, phrase, closeCypher, children, childrenSeparator);
        }

        #endregion // Composite

        #endregion // Cypher Operators

        #region Entity

        /// <summary>
        /// Node mutation by entity.
        /// </summary>
        public ICypherEntityMutations Entity => this;

        #endregion // Entity

        #region ICypherEntityMutations

        #region CreateNew

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="variable">
        /// The node's variable.
        /// When the parameter is null, it will be used as the parameter.
        /// </param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The parameter.</param>
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
        FluentCypher ICypherEntityMutations.CreateNew(
            string variable,
            IEnumerable<string> labels,
            string parameter)
        {
            parameter = parameter ?? variable;
            string labelsStr = _config.AmbientLabels.Combine(labels);
            return Create($"({variable}:{labelsStr} ${parameter})")
                    .Return(variable);
        }

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="variable">
        /// The node's variable.
        /// When the parameter is null, it will be used as the parameter.
        /// </param>
        /// <param name="label">The node's label which will be used for the parameter format (variable_label).</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("n", "FOO")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew("n", "FOO", "dev")
        /// Results in:
        /// CREATE (n:FOO:DEV $n_Foo) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateNew(
            string variable,
            string label,
            string? parameter)
        {
            ICypherEntityMutations self = this;
            return self.CreateNew(variable, label.AsYield(), parameter ?? variable);
        }


        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <typeparam name="T">will be used as the node's label. this label will also use for the parameter format (variable_typeof(T).Name).</typeparam>
        /// <param name="variable">
        /// The node's variable.
        /// When the parameter is null, it will be used as the parameter.
        /// </param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew<Foo>("n")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", "map")
        /// Results in:
        /// CREATE (n:FOO $n_map) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateNew<T>(
            string variable,
            string? parameter)
        {
            ICypherEntityMutations self = this;
            string label = typeof(T).Name;
            return self.CreateNew(variable, label, parameter);
        }

        #endregion // CreateNew

        #region CreateIfNotExists

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="variable">
        /// The node's variable.
        /// When the parameter is null, it will be used as the parameter.
        /// </param>
        /// <param name="labels">The labels.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map")
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists(
            string variable,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? parameter)
        {
            parameter = parameter ?? variable;
            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, parameter, ".");
            return Merge($"({variable}:{joinedLabel} {props})")
                        .OnCreate()
                        .SetEntity(variable, parameter, SetInstanceBehavior.Replace)
                        .Return(variable);
        }


        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists("p", "Person", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists(
            string variable,
            string label,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateIfNotExists(variable, label.AsYield(), matchProperty.ToYield(moreMatchProperties), parameter);
        }

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>("p", "map", "id", "name")
        /// Results in:
        /// MERGE (p:Person {id: $map.id, name: $map.name})
        ///     ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists<T>(
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateIfNotExists(variable, typeof(T).Name, parameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Create if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">
        /// The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.
        /// </param>
        /// <param name="parameter">The entity parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateIfNotExists<Person>(p => p.name, "map")
        /// MERGE (p:Person {name: $map.name})
        /// ON CREATE SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateIfNotExists<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntityMutations self = this;
            return self.CreateIfNotExists<T>(variable, parameter ?? variable, matchProperty);
        }

        #endregion // CreateInstanceIfNew

        #region AddOrModify

        /// <summary>
        /// Add or Modify entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="onMatchBehavior">The behavior.</param>
        /// <param name="parameterSign">The set prefix.</param>
        /// <param name="parent">The parent cypher.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// AddOrModify("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag", SetInstanceBehavior.Update)
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// ]]></example>
        private FluentCypher AddOrModify(
            string variable,
            IEnumerable<string> labels,
            string parameter,
            IEnumerable<string> matchProperties,
            SetInstanceBehavior onMatchBehavior,
            string parameterSign = "$",
            FluentCypher? parent = null)
        {
            string? eTagName = _config.Concurrency.eTagName;
            bool withConcurrency = !string.IsNullOrEmpty(eTagName);
            bool autoIncConcurrency = _config.Concurrency.AutoIncrement;
            string eTag = withConcurrency ? $"{variable}.{eTagName}" : string.Empty;

            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, parameter, ".", parameterSign);
            parent = parent ?? CypherBuilder.Default;
            var result = parent.Merge($"({variable}:{joinedLabel} {props})");

            if (withConcurrency)
            {
                result = result.Where($"{eTag} = {parameterSign}{parameter}.{eTagName}");
            }
            result = result.OnCreate()
                        .SetEntity(variable, parameter, parameterSign, SetInstanceBehavior.Replace);
            if (withConcurrency)
            {
                result = result.Add($", {eTag} = 0");
            }
            result = result.OnMatch()
                        .SetEntity(variable, parameter, parameterSign, onMatchBehavior);
            if (withConcurrency && autoIncConcurrency)
            {
                result = result.Add($", {eTag} = {eTag} + 1");
            }
            result = result.Return(variable);
            return result;
        }

        #endregion // AddOrModify

        #region CreateOrUpdate

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
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
        FluentCypher ICypherEntityMutations.CreateOrUpdate(
            string variable,
            IEnumerable<string> labels,
            string parameter,
            IEnumerable<string> matchProperties)
        {
            return AddOrModify(variable, labels, parameter, matchProperties,
                                SetInstanceBehavior.Update);
        }

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate("p", "Person", "map", "name")
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// CreateOrUpdate("p", "Person", "map", "name", "more")
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        /// SET p += $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate(
            string variable,
            string label,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrUpdate(variable, label.AsYield(), parameter, matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrUpdate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrUpdate<Person>("p", "map", "name")
        /// MERGE (p:Person {name: $map.name})
        /// SET p += $map
        /// CreateOrUpdate<Person>("p", "map", "name", "more")
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        /// SET p += $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateOrUpdate<T>(
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrUpdate(variable, typeof(T).Name, parameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Creates the or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">
        /// The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.
        /// </param>
        /// <param name="parameter">The entity parameter.</param>
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
        FluentCypher ICypherEntityMutations.CreateOrUpdate<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            parameter = parameter ?? variable;
            return AddOrModify(variable, typeof(T).Name.AsYield(), parameter, matchProperty.AsYield(),
                SetInstanceBehavior.Update);
        }

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Create or update entity.
        /// For update use CreateOrUpdate
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <returns></returns>
        /// make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
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
        FluentCypher ICypherEntityMutations.CreateOrReplace(
            string variable,
            IEnumerable<string> labels,
            string parameter,
            IEnumerable<string> matchProperties)
        {
            return AddOrModify(variable, labels, parameter, matchProperties,
                                SetInstanceBehavior.Replace);
        }

        /// <summary>
        /// Create or update entity.
        /// For update use CreateOrUpdate
        /// </summary>
        /// <param name="variable">The node variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace("p", "Person", "map", "name")
        /// MERGE (p:Person {name: $map.name})
        /// ON CREATE SET p = $map
        /// CreateOrReplace("p", "Person", "map", "name", "more")
        /// MERGE (p:Person {name: $map.name, more: $map.more})
        /// SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateOrReplace(
            string variable,
            string label,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrReplace(variable, label.AsYield(), parameter, matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Create or update entity.
        /// For update use CreateOrUpdate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The entity parameter.</param>
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
        FluentCypher ICypherEntityMutations.CreateOrReplace<T>(
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntityMutations self = this;
            return self.CreateOrReplace(variable, typeof(T).Name, parameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Creates the or replace.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchPropertyExpression">
        /// The match property expression.
        /// It will take the lambda variable as the expression variable.
        /// this variable will serve as the parameter when parameter is null.
        /// </param>
        /// <param name="parameter">The entity parameter.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace<Person>(p => p.name, "map")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        /// SET p = $map
        /// ---------------------------------------------------------
        /// CreateOrReplace<Person>(p => p.name, "map", "eTag")
        /// Results in:
        /// MERGE (p:Person {name: $map.name})
        /// SET p = $map, p.eTag = p.eTag + 1
        /// ]]></example>
        FluentCypher ICypherEntityMutations.CreateOrReplace<T>(
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            parameter = parameter ?? variable;
            return AddOrModify(variable, typeof(T).Name.AsYield(), parameter, matchProperty.AsYield(),
                                SetInstanceBehavior.Replace);
        }

        #endregion // CreateOrReplace

        #endregion // ICypherEntityMutations

        #region Entities

        /// <summary>
        /// Nodes mutation by entities (use Unwind pattern).
        /// </summary>
        public ICypherEntitiesMutations Entities => this;

        #endregion // Entities

        #region AddOrModifyCollection

        /// <summary>
        /// Add or Modify entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="collection">Name of the collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="item">The entity item.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="onMatchBehavior">The behavior.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// AddOrModify("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag", SetInstanceBehavior.Update)
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// ]]></example>
        private FluentCypher AddOrModifyCollection(
            string collection,
            IEnumerable<string> labels,
            string? variable,
            string? item,
            IEnumerable<string> matchProperties,
            SetInstanceBehavior onMatchBehavior)
        {
            item = item ?? Config.Pluralization.Singularize(collection);
            variable = variable ?? Char.ToLower(collection[0]).ToString();
            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, item, ".", string.Empty);
            var parent = Unwind(collection, item);
            FluentCypher result = AddOrModify(
                variable,
                labels,
                item,
                matchProperties,
                onMatchBehavior,
                string.Empty,
                parent);
            return result;
        }

        #endregion // AddOrModifyCollection

        #region ICypherEntitiesMutations

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
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateNew(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable,
            string? item)
        {
            item = item ?? Config.Pluralization.Singularize(collection);
            variable = variable ?? Char.ToLower(collection[0]).ToString();
            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, item, ".", string.Empty);

            var parent = _previous ?? CypherBuilder.Default;
            var cypher = parent
                .Unwind(collection, item)
                .Create($"({variable}:{joinedLabel} {props})")
                        .SetEntity(variable, item, string.Empty, SetInstanceBehavior.Replace)
                        .Return(variable);
            return new FluentCypherReturnProjection(cypher);
        }

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
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateNew(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateNew(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties));
        }

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
        FluentCypherReturnProjection<T> ICypherEntitiesMutations.CreateNew<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntitiesMutations self = this;
            var cypher = self.CreateNew(collection, 
                    typeof(T).Name.AsYield(),
                    matchProperty.AsYield(),
                    variable,
                    item);
            return new FluentCypherReturnProjection<T>(cypher);
        }

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
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateIfNotExists(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable,
            string? item)
        {
            item = item ?? Config.Pluralization.Singularize(collection);
            variable = variable ?? Char.ToLower(collection[0]).ToString();
            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, item, ".", string.Empty);
            
            var parent = _previous ?? CypherBuilder.Default;
            var cypher = parent
                .Unwind(collection, item)
                .Merge($"({variable}:{joinedLabel} {props})")
                        .OnCreate()
                        .SetEntity(variable, item, string.Empty, SetInstanceBehavior.Replace)
                        .Return(variable);

            return new FluentCypherReturnProjection(cypher);
        }


        FluentCypherReturnProjection ICypherEntitiesMutations.CreateIfNotExists(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateIfNotExists(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties));
        }

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
        FluentCypherReturnProjection<T> ICypherEntitiesMutations.CreateIfNotExists<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntitiesMutations self = this;
            var cypher = self.CreateIfNotExists(collection,
                    typeof(T).Name.AsYield(),
                    matchProperty.AsYield(),
                    variable,
                    item);
            return new FluentCypherReturnProjection<T>(cypher);
        }

        #endregion // CreateInstanceIfNew

        #region AddOrModify

        /// <summary>
        /// Add or Modify entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The entity parameter.</param>
        /// <param name="matchProperties">The match properties.</param>
        /// <param name="onMatchBehavior">The behavior.</param>
        /// <param name="parameterSign">The set prefix.</param>
        /// <param name="parent">The parent cypher.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// AddOrModify("p", new []{"Person", "Dev"}, new[] {"id", "name"}, "map", "eTag", SetInstanceBehavior.Update)
        /// Results in:
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p += $map, p.eTag = p.eTag + 1
        /// ]]></example>
        private FluentCypher AddOrModify(
            string collection,
            IEnumerable<string> labels,
            string variable,
            string parameter,
            IEnumerable<string> matchProperties,
            SetInstanceBehavior onMatchBehavior,
            string parameterSign = "$",
            FluentCypher? parent = null)
        {
            string? eTagName = _config.Concurrency.eTagName;
            bool withConcurrency = !string.IsNullOrEmpty(eTagName);
            bool autoIncConcurrency = _config.Concurrency.AutoIncrement;
            string eTag = withConcurrency ? $"{variable}.{eTagName}" : string.Empty;

            string joinedLabel = _config.AmbientLabels.Combine(labels);
            var props = P.Create(matchProperties, parameter, ".", parameterSign);
            parent = parent ?? CypherBuilder.Default;
            var result = parent
                .Unwind(collection, parameter)
                .Merge($"({variable}:{joinedLabel} {props})");

            if (withConcurrency)
            {
                result = result.Where($"{eTag} = ${parameter}.{eTagName}");
            }
            result = result.OnCreate()
                        .SetEntity(variable, parameter, parameterSign, SetInstanceBehavior.Replace);
            if (withConcurrency)
            {
                result = result.Add($", {eTag} = 0");
            }
            result = result.OnMatch()
                        .SetEntity(variable, parameter, parameterSign, onMatchBehavior);
            if (withConcurrency && autoIncConcurrency)
            {
                result = result.Add($", {eTag} = {eTag} + 1");
            }

            result = result.Return(variable); // TODO: Return projection
            return result;
        }

        #endregion // AddOrModify

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
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateOrUpdate(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable,
            string? item)
        {
            var result =  AddOrModifyCollection(collection, labels, variable, item, matchProperties,
                                SetInstanceBehavior.Update);
            return new FluentCypherReturnProjection(result);
        }

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
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateOrUpdate(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrUpdate(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties));
        }

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
        FluentCypherReturnProjection<T> ICypherEntitiesMutations.CreateOrUpdate<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntitiesMutations self = this;
            var result = self.CreateOrUpdate(collection,
                                typeof(T).Name.AsYield(),
                                matchProperty.AsYield(),
                                variable);
            return new FluentCypherReturnProjection<T>(result);
        }

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Batch Create or update entities.
        /// For replace use ReplaceOrReplace.
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
        /// CreateOrReplace("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: map.id, name: map.name})
        /// SET p = map
        /// RETURN p
        /// -------------------------------------------------------------
        /// CreateOrReplace("items", new []{"Person", "Dev"}, new[] {"id", "name"})
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person:Dev {id: item.id, name: item.name})
        /// SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateOrReplace(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string? variable,
            string? item)
        {
            var result =  AddOrModifyCollection(collection, labels, variable, item, matchProperties,
                                SetInstanceBehavior.Replace);
            return new FluentCypherReturnProjection(result);
        }

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrReplace.
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
        /// SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection ICypherEntitiesMutations.CreateOrReplace(
            string collection,
            string label,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrReplace(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties));
        }

        /// <summary>
        /// Creates the or update.
        /// For update use ReplaceOrReplace.
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
        /// -------------------------------------------
        /// CreateOrReplace<Person>("items", p => p.name)
        /// Results in:
        /// UNWIND items as item 
        /// MERGE (i:Person {name: item.name})
        /// SET i = item
        /// RETURN i
        /// ]]></example>
        FluentCypherReturnProjection<T> ICypherEntitiesMutations.CreateOrReplace<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? item)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);

            ICypherEntitiesMutations self = this;
            var result = self.CreateOrReplace(collection,
                                typeof(T).Name.AsYield(),
                                matchProperty.AsYield(),
                                variable);
            return new FluentCypherReturnProjection<T>(result);
        }

        #endregion // CreateOrReplace

        #endregion // ICypherEntitiesMutations

        // ------------------------------------------------------------

        #region Config

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        internal protected CypherConfig _config { get; }
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public ICypherConfig Config => _config;

        #endregion // Config

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
        private static StringBuilder AccumulateForward(
            StringBuilder sb,
            FluentCypher current,
            CypherFormat cypherFormat)
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

                if (prevPhrase == CypherPhrase.None || prevPhrase == CypherPhrase.Or || prevPhrase == CypherPhrase.And)
                {
                    //phrase = previous._phrase;
                    previous = previous._previous;
                    continue;
                }

                if (phrase == CypherPhrase.Project)
                    phrase = CypherPhrase.Return;
                if (prevPhrase == CypherPhrase.Project)
                    prevPhrase = CypherPhrase.Return;
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
