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
using System.Collections.Immutable;

namespace Weknow
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    public class C : CypherBuilder
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="C"/> class from being created.
        /// </summary>
        private protected C()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="C"/> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        protected internal C(string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="C"/> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        private protected C(FluentCypher copyFrom, string cypher, CypherPhrase phrase, string? cypherClose = null, IEnumerable<FluentCypher>? children = null, string? childrenSeparator = null) : base(copyFrom, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }
    }

    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypherWhereExpression" />
    /// <seealso cref="Weknow.ICypherEntityMutations" />
    /// <seealso cref="Weknow.ICypherEntitiesMutations" />
    /// <seealso cref="Weknow.ICypherLabelContext" />
    /// <seealso cref="Weknow.FluentCypher" />
    public class CypherBuilder :
        FluentCypherWhereExpression,
        ICypherEntityMutations,
        ICypherEntitiesMutations
    {
        #region static Create

        /// <summary>
        /// Root Cypher Builder.
        /// </summary>
        public static FluentCypher Create(Action<CypherConfig>? config = null)
        {
            var configuration = new CypherConfig();
            config?.Invoke(configuration);
            return new CypherBuilder(configuration);
        }

        #endregion // static Create

        #region static Create

        /// <summary>
        /// Root Cypher Builder.
        /// </summary>
        internal static readonly FluentCypher Default = new CypherBuilder();

        #endregion // static Create

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="FluentCypher" /> class from being created.
        /// </summary>
        internal CypherBuilder()
        {
        }

        /// <summary>
        /// Initialize constructor
        /// </summary>
        /// <param name="config">The configuration.</param>
        private protected CypherBuilder(CypherConfig config)
            : base(config)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder" /> class.
        /// </summary>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        internal protected CypherBuilder(
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(Default, cypher, phrase, cypherClose, children, childrenSeparator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherBuilder" /> class.
        /// </summary>
        /// <param name="copyFrom">The copy from.</param>
        /// <param name="cypher">The cypher.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="cypherClose">The cypher close.</param>
        /// <param name="children">The children.</param>
        /// <param name="childrenSeparator">The children separator.</param>
        private protected CypherBuilder(
            FluentCypher copyFrom,
            string cypher,
            CypherPhrase phrase,
            string? cypherClose = null,
            IEnumerable<FluentCypher>? children = null,
            string? childrenSeparator = null)
            : base(copyFrom, cypher, phrase, cypherClose,
                  children, childrenSeparator)
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
        /// <example><![CDATA[
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// MATCH (n)-->(m)
        /// MATCH (n {name: 'Alice'})-->(m)
        /// ]]></example>
        public override FluentCypher Match(string statement) => AddStatement(statement, CypherPhrase.Match);

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
        public override FluentCypher OptionalMatch(string statement) => AddStatement(statement, CypherPhrase.OptionalMatch);

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
        public override FluentCypher Merge(string statement) => AddStatement(statement, CypherPhrase.Merge);

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
        public override FluentCypher OnCreate() => AddStatement(CypherPhrase.OnCreate);

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
        public override FluentCypher OnCreate(string statement) => AddStatement(statement, CypherPhrase.OnCreate);

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
        public override FluentCypher OnMatch() => AddStatement(CypherPhrase.OnMatch);

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
        public override FluentCypher OnMatch(string statement) => AddStatement(statement, CypherPhrase.OnMatch);

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
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
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
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
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
        /// <example><![CDATA[
        /// MERGE (n:Person {name: $value})
        /// ON CREATE SET n.created = timestamp()
        /// ON MATCH SET
        /// n.counter = coalesce(n.counter, 0) + 1,
        /// n.accessTime = timestamp()
        /// ]]></example>
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
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// REMOVE n:Person // Remove a label from n.
        /// REMOVE n.property // Remove a property.
        /// ]]></example>
        public override FluentCypher Remove(string statement) => AddStatement(statement, CypherPhrase.Remove);

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
        public override FluentCypher Delete(string statement) => AddStatement(statement, CypherPhrase.Delete);

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
        public override FluentCypher Unwind(string collection, string variable, string collectionSign = "$") =>
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
        public override FluentCypherReturn With(string statement) =>
                            AddStatement(statement, CypherPhrase.With);

        #endregion // With

        #region Return 

        /// <summary>
        /// Create RETURN phrase.
        /// </summary>
        /// <returns></returns>
        public override FluentCypherReturn Return() =>
                            AddStatement(CypherPhrase.Return);

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
        public override FluentCypherReturn Return(string statement) =>
                            AddStatement(statement, CypherPhrase.Return);

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
        /// <example><![CDATA[
        /// RETURN DISTINCT n // Return unique rows.
        /// ]]></example>
        public override FluentCypherReturn ReturnDistinct(string statement) =>
                            AddStatement(statement, CypherPhrase.ReturnDistinct);

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
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
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
        /// <example><![CDATA[
        /// MATCH (a)-[:KNOWS]->(b)
        /// RETURN b.name
        /// UNION All
        /// MATCH (a)-[:LOVES]->(b)
        /// RETURN b.name
        /// ]]></example>
        public override FluentCypher UnionAll() =>
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
        public override FluentCypher Call(string statement) =>
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
        public override FluentCypher Set(string statement) =>
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
        public override FluentCypher Set(
            string variable,
            IEnumerable<string> propNames,
            string? parameterPrefix,
            string parameterSign)
        {
            CypherBuilder result = propNames.FormatSetWhere(variable, parameterPrefix, parameterSign)
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
        /// <param name="parameterPrefix">The parameter prefix.</param>
        /// <param name="parameterSign">The parameter sign.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// Set((User user) => user.Name)
        /// SET user.Name = $Name // Update or create a property.
        /// ]]></example>
        public override FluentCypherSet<T> Set<T>(
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

        #region SetEntity

        /// <summary>
        /// Sets the entity.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="parameterSign">The parameter sign ($ or nothing).</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public override FluentCypher SetEntity(
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
        public override FluentCypher SetEntity(
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
        public override FluentCypher SetEntity<T>(
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
        /// <param name="variable">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SET n:Person
        /// ]]></example>
        public override FluentCypher SetLabel(string variable, string label)
        {
            string statement = $"{variable}:{Config.Labels.Format(label)}";
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
        public override FluentCypherWhereExpression Where(string statement) =>
                                            AddStatement(statement, CypherPhrase.Where);

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
        /// <example><![CDATA[
        /// Where ("n", new [] {"A", "B"})
        /// Result with
        /// WHERE n.A = $n_A AND n.B = $n_B
        /// ]]></example>
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
        public override FluentCypherWhereExpression Where<T>(
                    Expression<Func<T, dynamic>> propExpression,
                    string? parameterPrefix,
                    string parameterSign,
                    string compareSign)
        {
            (string variable, string name) = ExtractLambdaExpression(propExpression);
            string statement = $"{variable}.{name}";
            statement = $"{statement} {compareSign} {parameterSign}{parameterPrefix}{name}";
            return AddStatement<T>(statement, CypherPhrase.Where);
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
        public override FluentCypher ForEach(string statement) =>
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
        /// <example><![CDATA[
        /// ForEach("n", "nations", new [] {nameof(Foo.Name), nameof(Bar.Id)})
        /// FOREACH (n IN nations | SET n.Name = $n.Name, n.Id = $n.Id)
        /// ]]></example>
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
        /// <example><![CDATA[
        /// ForEach("$users", name =&gt; name.EndsWith("Name"))
        /// ForEach(user IN $users | SET user.FirstName = $user.FirstName, user.LastName = $user.LastName) // Update or create a property.
        /// ]]></example>
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
        /// <example><![CDATA[
        /// ORDER BY n.property
        /// ]]></example>
        public override FluentCypherReturn OrderBy(string statement) =>
                            AddStatement(statement, CypherPhrase.OrderBy);

        #endregion // OrderBy

        #region OrderByDesc

        /// <summary>
        /// Create ORDER BY DESC phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// ORDER BY n.property DESC
        /// ]]></example>
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
        /// <example><![CDATA[
        /// SKIP $skipNumber
        /// ]]></example>
        public override FluentCypherReturn Skip(string statement) =>
                            AddStatement(statement, CypherPhrase.Skip);

        /// <summary>
        /// Create SKIP phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// SKIP 10
        /// ]]></example>
        public override FluentCypherReturn Skip(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Skip);

        #endregion // Skip

        #region Limit

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// LIMIT $skipNumber
        /// ]]></example>
        public override FluentCypherReturn Limit(string statement) =>
                    AddStatement(statement, CypherPhrase.Limit);

        /// <summary>
        /// Create LIMIT phrase.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// LIMIT 10
        /// ]]></example>
        public override FluentCypherReturn Limit(int number) =>
                            AddStatement(number.ToString(), CypherPhrase.Limit);

        #endregion // Limit

        #region As

        /// <summary>
        /// Create As phrase
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// collect(list) AS items
        /// /// ]]></example>
        public override FluentCypher As(string name) => Add($"AS {name}");

        #endregion // As

        #region Count

        /// <summary>
        /// Create count function.
        /// </summary>
        /// <returns></returns>
        /// <example><![CDATA[
        /// RETURN count(*)
        /// ]]></example>
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

        #region Entity

        /// <summary>
        /// Node mutation by entity.
        /// </summary>
        public override ICypherEntityMutations Entity => this;

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
            string labelsStr = Config.Labels.Format(labels);
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
            string joinedLabel = Config.Labels.Format(labels);
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
            string? eTagName = Config.Concurrency.eTagName;
            bool withConcurrency = !string.IsNullOrEmpty(eTagName);
            bool autoIncConcurrency = Config.Concurrency.AutoIncrement;
            string eTag = withConcurrency ? $"{variable}.{eTagName}" : string.Empty;

            string joinedLabel = Config.Labels.Format(labels);
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
        public override ICypherEntitiesMutations Entities => this;

        #endregion // Entities

        #region AddOrModifyCollection

        /// <summary>
        /// Add or Modify entity.
        /// For replace use ReplaceOrUpdate.
        /// </summary>
        /// <param name="collection">Name of the collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The entity parameter.</param>
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
            string variable,
            string ?parameter,
            IEnumerable<string> matchProperties,
            SetInstanceBehavior onMatchBehavior)
        {
            parameter = parameter ?? ToParameterConvention(variable);
            string joinedLabel = Config.Labels.Format(labels);
            var props = P.Create(matchProperties, parameter, ".", string.Empty);
            var parent = Unwind(collection, parameter);
            FluentCypher result = AddOrModify(
                variable,
                labels,
                parameter,
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
        /// CREATE by entity
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="labels">The labels.</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateNew("items", "n", new [] {"A", "B"}, "map")
        /// Results in:
        /// Unwind $items as map
        /// CREATE (n:A:B $n_map)
        /// ----------------------------------------------------------
        /// CreateNew("n", new [] {"A", "B"}, "map", "prefix")
        /// Results in:
        /// Unwind $items as map
        /// CREATE (n:A:B $prefix_map)
        /// ----------------------------------------------------------
        /// CreateNew("n", new [] {"A", "B"}, "map", "prefix", ".")
        /// Results in:
        /// Unwind $items as map
        /// CREATE (n:A:B $prefix.map)
        /// ]]></example>
        FluentCypher ICypherEntitiesMutations.CreateNew(
            string collection,
            IEnumerable<string> labels,
            string variable,
            string? parameter)
        {
            parameter = parameter ?? ToParameterConvention(variable);
            string labelsStr = Config.Labels.Format(labels);
            return Unwind(collection, parameter)
                    .Create($"({variable}:{labelsStr} {parameter})")
                    .Return(variable); // TODO: Return projection
        }

        /// <summary>
        /// CREATE by entity
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The node's label which will be used for the parameter format (variable_label).</param>
        /// <param name="variable">The node's variable.
        /// When the parameter is null, it will be used as the parameter.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
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
        FluentCypher ICypherEntitiesMutations.CreateNew(
            string collection,
            string label,
            string variable,
            string? parameter)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateNew(collection, label.AsYield(), variable, parameter ?? variable);
        }


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
        /// CreateNew<Foo>("n")
        /// Results in:
        /// CREATE (n:FOO $n_Foo) // Create a node with the given properties.
        /// --------------------------------------------------------------------------
        /// CreateNew<Foo>("n", "map")
        /// Results in:
        /// CREATE (n:FOO $n_map) // Create a node with the given properties.
        /// ]]></example>
        FluentCypher ICypherEntitiesMutations.CreateNew<T>(
            string collection,
            string variable,
            string? parameter)
        {
            ICypherEntitiesMutations self = this;
            string label = typeof(T).Name;
            return self.CreateNew(collection, label, variable, parameter);
        }

        #endregion // CreateNew

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
        FluentCypher ICypherEntitiesMutations.CreateIfNotExists(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable,
            string? parameter)
        {
            parameter = parameter ?? ToParameterConvention(variable);
            string joinedLabel = Config.Labels.Format(labels);
            var props = P.Create(matchProperties, parameter, ".", string.Empty);
            return Unwind(collection, parameter)
                .Merge($"({variable}:{joinedLabel} {props})")
                        .OnCreate()
                        .SetEntity(variable, parameter, string.Empty, SetInstanceBehavior.Replace)
                        .Return(variable); // TODO: Return projection
        }


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
        FluentCypher ICypherEntitiesMutations.CreateIfNotExists(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateIfNotExists(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties), variable, parameter);
        }

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
        FluentCypher ICypherEntitiesMutations.CreateIfNotExists<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateIfNotExists(collection, typeof(T).Name, variable, parameter, matchProperty, moreMatchProperties);
        }

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
        FluentCypher ICypherEntitiesMutations.CreateIfNotExists<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            parameter = parameter ?? ToParameterConvention(variable);

            ICypherEntitiesMutations self = this;
            return self.CreateIfNotExists<T>(collection, variable, parameter, matchProperty);
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
            string? eTagName = Config.Concurrency.eTagName;
            bool withConcurrency = !string.IsNullOrEmpty(eTagName);
            bool autoIncConcurrency = Config.Concurrency.AutoIncrement;
            string eTag = withConcurrency ? $"{variable}.{eTagName}" : string.Empty;

            string joinedLabel = Config.Labels.Format(labels);
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
        FluentCypher ICypherEntitiesMutations.CreateOrUpdate(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable,
            string? parameter)
        {
            return AddOrModifyCollection(collection, labels, variable, parameter, matchProperties,
                                SetInstanceBehavior.Update);
        }

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
        FluentCypher ICypherEntitiesMutations.CreateOrUpdate(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrUpdate(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties), variable, parameter);
        }

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
        FluentCypher ICypherEntitiesMutations.CreateOrUpdate<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrUpdate(collection, typeof(T).Name, variable, parameter, matchProperty, moreMatchProperties);
        }

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
        FluentCypher ICypherEntitiesMutations.CreateOrUpdate<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            parameter = parameter ?? ToParameterConvention(variable);
            ICypherEntitiesMutations self = this;
            return self.CreateOrUpdate(collection,
                                typeof(T).Name.AsYield(),
                                matchProperty.AsYield(),
                                variable,
                                parameter);
        }

        #endregion // CreateOrUpdate

        #region CreateOrReplace

        /// <summary>
        /// Batch Create or update entities.
        /// For replace use ReplaceOrReplace
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
        /// CreateOrReplace("items", new []{"Person", "Dev"}, new[] {"id", "name"}, "p", "map")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person:Dev {id: $map.id, name: $map.name})
        /// SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntitiesMutations.CreateOrReplace(
            string collection,
            IEnumerable<string> labels,
            IEnumerable<string> matchProperties,
            string variable,
            string? parameter)
        {
            return AddOrModifyCollection(collection, labels, variable, parameter, matchProperties,
                                SetInstanceBehavior.Replace);
        }

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrReplace.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="label">The label.</param>
        /// <param name="variable">The node variable.</param>
        /// <param name="parameter">The parameter (if missing, use the variable instead).</param>
        /// <param name="matchProperty">The match property.</param>
        /// <param name="moreMatchProperties">The more match properties.</param>
        /// <returns></returns>
        /// <example><![CDATA[
        /// CreateOrReplace("items", "Person", "p", "map", "name")
        /// Results in:
        /// UNWIND items as map 
        /// MERGE (p:Person {name: $map.name})
        /// SET p = $map
        /// ]]></example>
        FluentCypher ICypherEntitiesMutations.CreateOrReplace(
            string collection,
            string label,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrReplace(collection, label.AsYield(), matchProperty.ToYield(moreMatchProperties), variable, parameter);
        }

        /// <summary>
        /// Create or update entity.
        /// For replace use ReplaceOrReplace
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
        FluentCypher ICypherEntitiesMutations.CreateOrReplace<T>(
            string collection,
            string variable,
            string parameter,
            string matchProperty,
            params string[] moreMatchProperties)
        {
            ICypherEntitiesMutations self = this;
            return self.CreateOrReplace(collection, typeof(T).Name, variable, parameter, matchProperty, moreMatchProperties);
        }

        /// <summary>
        /// Creates the or update.
        /// For update use ReplaceOrReplace.
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
        FluentCypher ICypherEntitiesMutations.CreateOrReplace<T>(
            string collection,
            Expression<Func<T, dynamic>> matchPropertyExpression,
            string? parameter)
        {
            var (variable, matchProperty) = ExtractLambdaExpression(matchPropertyExpression);
            parameter = parameter ?? ToParameterConvention(variable);

            ICypherEntitiesMutations self = this;
            return self.CreateOrReplace(collection,
                                typeof(T).Name.AsYield(),
                                matchProperty.AsYield(),
                                variable,
                                parameter);
        }

        #endregion // CreateOrReplace

        #endregion // ICypherEntitiesMutations
    }
}