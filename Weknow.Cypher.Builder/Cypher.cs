using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Weknow.GraphDbCommands.Declarations;

using static Weknow.GraphDbCommands.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

// TODO: [bnaya 2022-10-20] source code generator of the cypher
namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Entry point for constructing root level Cypher.
    /// For fluent cypher check <see cref="CypherPhraseExtensions" />
    /// </summary>
    public static partial class Cypher
    {
        #region HandleConfigInjection

        /// <summary>
        /// Handles the configuration injection.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cfg">The CFG.</param>
        private static void HandleConfigInjection(
            Action<CypherConfig>? configuration, CypherConfig cfg)
        {
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
        }

        #endregion // HandleConfigInjection

        #region Init

        /// <summary>
        /// Initializes a builder.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static CypherCommand Init(CypherConfig cfg, Expression expression)
        {
            using var visitor = new CypherVisitor(cfg);

            visitor.Visit(expression);
            string cypher = visitor.Query.ToString().Replace("\r\n", Environment.NewLine);
            CypherParameters parameters = visitor.Parameters;

            return new CypherCommand(
                            cypher, 
                            parameters);
        }

        #endregion // Init

        #region dash '_'

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _(
                            Expression<NoVariable> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _(
                            Expression<Fluent> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        #endregion // dash '_'

        #region IPattern N (Node)

        /// <summary>
        /// Specified node with label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        [Cypher("(:$0)")]
        public static INode N(ILabel label) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("($0)")]
        public static INode N(VariableDeclaration var) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable and label.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        [Cypher("($0$1)")]
        public static INode N(VariableDeclaration var, ILabel label) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable, label and properties.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher("($0$1 $2)")]
        public static INode N(VariableDeclaration var, ILabel label, object properties) => throw new NotImplementedException();
        [Cypher("($0 $1)")]
        public static INode N(VariableDeclaration var, object properties) => throw new NotImplementedException();
        [Cypher("($0 $1)")]
        public static INode N(ILabel label, object properties) => throw new NotImplementedException();

        #endregion // IPattern N (Node)

        #region IRelation R (Relation)

        /// <summary>
        /// Gets the Cypher relation.
        /// </summary>
        public static IRelation R => throw new NotImplementedException();

        #endregion // IRelation R (Relation)

        ///// <summary>
        ///// Used to define properties' variables without the $ prefix.
        ///// Useful for cases like UNWIND
        ///// </summary>
        ///// <param name="var">The variable.</param>
        ///// <returns></returns>
        ///// <example>
        ///// UNWIND $items AS item 
        /////     MATCH(n:Person { PropA: item.x })
        ///// </example>
        //[Cypher("$0\\.$1")]
        //public static T _<T>(this VariableDeclaration var) => throw new NotImplementedException();

        ///// <summary>
        ///// Used to define properties' variables without the $ prefix.
        ///// Useful for cases like UNWIND
        ///// </summary>
        ///// <param name="var">The variable.</param>
        ///// <param name="exp">Any generics expression</param>
        ///// <returns></returns>
        ///// <example>
        ///// UNWIND $items AS item 
        /////     MATCH(n:Person { PropA: item.x })
        ///// </example>
        //[Cypher("$0\\.$1")]
        public static T _<T>(this VariableDeclaration var, T exp) => throw new NotImplementedException();

        #region Rgx

        /// <summary>
        /// Use regular expression comparison under WHERE clause.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <example>
        /// WHERE n.name =~ 'Tim.*'
        /// </example>
        [Cypher("&$0")]
        public static bool Rgx(bool condition) => throw new NotImplementedException();
        /// <summary>
        /// Use regular expression comparison under WHERE clause.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// WHERE n.name =~ 'Tim.*'
        /// </example>
        [Cypher("&$0")]
        public static bool Rgx(object properties) => throw new NotImplementedException();

        #endregion // Rgx

        #region Profile

        /// <summary>
        /// Profile execution.
        /// </summary>
        /// <returns></returns>
        [Cypher("PROFILE")]
        public static Fluent Profile() => throw new NotImplementedException();

        #endregion // Profile

        #region Match

        /// <summary>
        /// Matches phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// </example>
        [Cypher("MATCH $0")]
        public static Fluent Match(params INode[] p) => throw new NotImplementedException();

        /// <summary>
        /// Matches phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// MATCH (n:Person)-[:KNOWS]->(m:Person)
        /// </example>
        [Cypher("OPTIONAL MATCH $0")]
        public static Fluent OptionalMatch(params INode[] p) => throw new NotImplementedException();

        #endregion // Match

        #region Create

        /// <summary>
        /// Create phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// CREATE (n {name: $value})
        /// </example>
        [Cypher("CREATE $0")]
        public static Fluent Create(IPattern p) => throw new NotImplementedException();

        #endregion // Create

        #region Merge

        /// <summary>
        /// MERGE phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// MERGE (n:Person {name: $value})
        /// </example>
        [Cypher("MERGE $0")]
        public static Fluent Merge(IPattern p) => throw new NotImplementedException();

        #endregion // Merge

        #region Unwind

        /// <summary>
        /// UNWIND phrase.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="item">The item.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// UNWIND $names AS name
        /// MATCH(n { name: name})
        /// RETURN avg(n.age)
        /// </example>
        [Cypher("&UNWIND \\$$0 AS $1\r\n$2")]
        public static Fluent Unwind(VariableDeclaration items, VariableDeclaration item, Fluent p) => throw new NotImplementedException();

        /// <summary>
        /// Unwinds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="item">The item.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("&UNWIND $0 AS $1\r\n$2")]
        public static Fluent Unwind(ParameterDeclaration items, VariableDeclaration item, Fluent p) => throw new NotImplementedException();
        #endregion // Unwind

        #region Exists

        /// <summary>
        /// EXISTS phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// exists(n.property)
        /// </example>
        [Cypher("EXISTS { $0 }")]
        public static bool Exists(Fluent p) => throw new NotImplementedException();
        /// <summary>
        /// EXISTS phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// exists(n.property)
        /// </example>
        [Cypher("EXISTS { $0 }")]
        public static bool Exists(NoVariable p) => throw new NotImplementedException();

        #endregion // Exists

        #region Reuse

        /// <summary>
        /// Reuses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelationNode Reuse(
                            Expression<Func<IRelationNode>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new RelationNodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Reuses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INodeRelation Reuse(
                            Expression<Func<INodeRelation>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new NodeRelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INode Reuse(
                            Expression<Func<INode>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
            return new NodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelation Reuse(
                            Expression<Func<IRelation>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new RelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INodeRelation Reuse(
                            Expression<Func<Func<VariableDeclaration, INodeRelation>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
            return new NodeRelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INode Reuse(
                            Expression<Func<Func<VariableDeclaration, INode>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new NodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelation Reuse(
                            Expression<Func<Func<VariableDeclaration, IRelation>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
            return new RelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INode Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, INode>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new NodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelation Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
            return new RelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INode Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, INode>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new NodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelation Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            CypherConfig.Scope.Value?.Invoke(cfg);
            configuration?.Invoke(cfg);
            return new RelationPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static INode Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, INode>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new NodePattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IRelation Reuse(
                            Expression<Func<Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, Func<VariableDeclaration, IRelation>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            HandleConfigInjection(configuration, cfg);
            return new RelationPattern(expression.Body, cfg);
        }

        #endregion // Reuse

        #region Timestamp / timestamp()

        /// <summary>
        /// Milliseconds since midnight, January 1, 1970 UTC.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// MATCH (n)
        /// RETURN timestamp()
        /// </example>
        [Cypher("timestamp()")]
        public static VariableDeclaration Timestamp() => throw new NotImplementedException();

        #endregion // Timestamp / timestamp()
    }
}
