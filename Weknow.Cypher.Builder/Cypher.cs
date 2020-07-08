using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Entry point for constructing root level Cypher.
    /// For fluent cypher check <see cref="CypherExtensions"/>
    /// </summary>
    public static class Cypher
    {
        #region Init

        /// <summary>
        /// Initializes a builder.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static CypherCommand Init(CypherConfig cfg, Expression expression)
        {
            var visitor = new CypherVisitor(cfg);
            visitor.Visit(expression);
            return new CypherCommand(
                            visitor.Query.ToString(), // TODO: format according to the configuration
                            visitor.Parameters);
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
                            Expression<Fluent> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T>(
                            Expression<Project<T, Fluent>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2>(
                            Expression<Project<T1, Project<T2, Fluent>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3>(
                            Expression<Project<T1, Project<T2, Project<T3, Fluent>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }


        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4>(
                            Expression<Project<T1, Project<T2, Project<T3, Project<T4, Fluent>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4, T5>(
                            Expression<Project<T1, Project<T2, Project<T3, Project<T4, Project<T5, Fluent>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        /// <summary>
        /// Build cypher expression
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static CypherCommand _<T1, T2, T3, T4, T5, T6>(
                            Expression<Project<T1, Project<T2, Project<T3, Project<T4, Project<T5, Project<T6, Fluent>>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
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
                            Expression<NoVariable> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            CypherCommand result = Init(cfg, expression);
            return result;
        }

        #endregion // dash '_'

        // TODO: [bnaya, 2020-07] Test each IPattern N (Node) and add example to the documentation
        #region IPattern N (Node)

        /// <summary>
        /// Specified node with label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        [Cypher("(:$0)")]
        public static IPattern N(ILabel label) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("($0)")]
        public static IPattern N(IVar var) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable and label.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        [Cypher("($0$1)")]
        public static IPattern N(IVar var, ILabel label) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable, label and properties.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher("($0$1 { $2 })")]
        public static IPattern N(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable, label and properties.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties"></param>
        /// <returns></returns>
        [Cypher("($0$1 { $2 })")]
        public static IPattern N(IVar var, ILabel label, params IProperty[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable, label and map.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="map">The properties.</param>
        /// <returns></returns>
        [Cypher("($0$1 $2)")]
        public static IPattern N(IVar var, ILabel label, IMap map) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("($0:!l0)")]
        public static IPattern N<T>(IVar var) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with variable and properties map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="map">The properties.</param>
        /// <returns></returns>
        [Cypher("($0:!l0 $1)")]
        public static IPattern N<T>(IVar var, IMap map) => throw new NotImplementedException();


        /// <summary>
        /// Specified typed node with variable and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar var, Func<T, IProperties> properties) => throw new NotImplementedException();

        /// <summary>
        /// Specified typed node with typed variable and label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar<T> var, Func<T, IProperties> properties) => throw new NotImplementedException();

        /// <summary>
        /// Specified typed node with variable and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar var, IProperties properties) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with typed variable and label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar<T> var, IProperties properties) => throw new NotImplementedException();

        /// <summary>
        /// Specified typed node with variable and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="property">.</param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar var, IProperty property) => throw new NotImplementedException();

        /// <summary>
        /// Specified typed node with typed variable and label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="property"></param>
        /// <returns></returns>
        [Cypher(".1($0:!l0 { $1 })")]
        public static IPattern N<T>(IVar<T> var, IProperty property) => throw new NotImplementedException();

        /// <summary>
        /// Specified typed node with variable and label.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        [Cypher("($0:!0$1)")]
        public static IPattern N<T>(IVar var, ILabel label) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with variable, label and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher("($0:!0$1 { $2 })")]
        public static IPattern N<T>(IVar var, ILabel label, IProperties properties) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with variable, label and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [Cypher("($0:!0$1 { $2 })")]
        public static IPattern N<T>(IVar var, ILabel label, Func<T, IProperties> properties) => throw new NotImplementedException();
        /// <summary>
        /// Specified typed node with variable, label and properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="genericAslabel">Indicate whether to use the generics argument as label.</param>
        /// <returns></returns>
        [Cypher("($0:!0$1 { $2 })")]
        public static IPattern N<T>(IVar var, ILabel label, Func<T, IProperties> properties, LabelFromGenerics genericAslabel) => throw new NotImplementedException();
        /// <summary>
        /// Specified node with variable, label and map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="label">The label.</param>
        /// <param name="map">The properties.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("($0:!0$1 $2)")]
        public static IPattern N<T>(IVar var, ILabel label, IMap map) => throw new NotImplementedException();

        #endregion // IPattern N (Node)

        #region IRelation R (Relation)

        /// <summary>
        /// Gets the Cypher relation.
        /// </summary>
        public static IRelation R => throw new NotImplementedException();

        #endregion // IRelation R (Relation)

        #region IProperties P (Properties)

        /// <summary>
        /// Represent  properties collection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// {name: $name, value: $value}
        /// </example>
        [Cypher("$0")]
        public static IProperties P(params IProperty[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent  properties collection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// {name: $name, value: $value}
        /// </example>
        [Cypher("$0")]
        public static IPropertiesOfType P(params object[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent  properties collection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// {name: $name, value: $value}
        /// </example>
        [Cypher("$0")]
        public static IPropertiesOfType P<T>(params Func<T, object>[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent  properties collection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// {name: $name, value: $value}
        /// </example>
        [Cypher("$0")]
        public static IProperties P(Expression<Func<IProperties>> properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent variable with properties collection.
        /// <param name="var"></param>
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// </example>
        [Cypher("+30$1")]
        public static IPropertiesOfType P(this IVar var, params object[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent variable with properties collection.
        /// <param name="var"></param>
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// </example>
        [Cypher("+30$1")]
        public static IPropertiesOfType P(this IVar var, params IProperty[] properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent variable with properties collection.
        /// <param name="var"></param>
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// </example>
        [Cypher("+30$1")]
        public static IPropertiesOfType P<T>(this IVar var, Func<T, object> properties) => throw new NotImplementedException();
        /// <summary>
        /// Set property with variable (useful for unwind's variable).
        /// <param name="var"></param>
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <example>
        /// UNWIND $items AS item
        /// MERGE(n:Person { Id: item })
        /// RETURN n
        /// </example>
        [Cypher("$0: $1")]
        public static IPropertiesConst P(IProperty properties, IVar var) => throw new NotImplementedException();

        #endregion // IProperties P (Properties)

        #region IProperties _P (Properties with prefix)

        /// <summary>
        /// Represent properties with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <example>
        /// { Name: $pre_Name }
        /// </example>
        [Cypher("+00$1")]
        public static IPropertiesOfType _P(IVar prefix, IProperties properties) => throw new NotImplementedException();
        /// <summary>
        /// Represent property with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <example>
        /// { Name: $pre_Name }
        /// </example>
        [Cypher("+00$1")]
        public static IProperty _P(IVar prefix, IProperty property) => throw new NotImplementedException();

        #endregion // IProperties _P (Properties with prefix)

        #region IProperties _P (Properties with prefix)

        /// <summary>
        /// Represent property with prefix.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        /// <example>
        /// { Id: $Key }
        /// </example>
        [Cypher("$0")]
        public static IProperty P_(IProperty property, IVar variableName) => throw new NotImplementedException();

        /// <summary>
        /// Represent property with prefix.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        /// <example>
        /// { Id: $Key }
        /// </example>
        [Cypher("$0")]
        public static IProperty P_<T>(IProperty property, Func<T, object> variableName) => throw new NotImplementedException();

        #endregion // IProperties _P (Properties with prefix)

        #region IProperties Convention (Properties by convention)

        /// <summary>
        /// Gets properties by convention.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <example>
        /// </example>
        public static IProperties Convention(Func<string, bool> filter) => throw new NotImplementedException();

        /// <summary>
        /// Gets properties by convention with variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example></example>
        public static IPropertiesOfType Convention<T>(this IVar var, Func<string, bool> filter) => throw new NotImplementedException();
        /// <summary>
        /// Gets properties by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <example></example>
        public static IProperties Convention<T>(Func<string, bool> filter) => throw new NotImplementedException();

        #endregion // IProperties Convention (Properties by convention)

        #region IProperties All (All Properties)

        /// <summary>
        /// All properties of the type.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProperties All() => throw new NotImplementedException();
        /// <summary>
        /// All properties of the type.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IPropertiesOfType All<T>(this IVar var) => throw new NotImplementedException();

        #endregion // IProperties All (All Properties)

        #region IProperties AllExcept (All properties except)

        /// <summary>
        /// All properties of the type except the ones specify in the parameters.
        /// </summary>
        /// <param name="except">The except.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProperties AllExcept(params object[] except) => throw new NotImplementedException();

        #endregion // IProperties AllExcept (All properties except)

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
        public static Fluent Match(params IPattern[] p) => throw new NotImplementedException();

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
        [Cypher("&UNWIND \\$$0 AS $1\r\n+21$2")]
        public static Fluent Unwind(IVar items, IVar item, Fluent p) => throw new NotImplementedException();
        /// <summary>
        /// UNWIND phrase.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// UNWIND $names AS name
        /// MATCH(n { name: name})
        /// RETURN avg(n.age)
        /// </example>
        [Cypher("&UNWIND \\$$0 AS $s0\r\n+s20$1")]
        public static Fluent Unwind(IVar items, Fluent p) => throw new NotImplementedException();

        #endregion // Unwind

        #region Exists

        /// <summary>
        /// UNWIND phrase.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <example>
        /// exists(n.property)
        /// </example>
        [Cypher("EXISTS { $0 }")]
        public static bool Exists(Fluent p) => throw new NotImplementedException();

        #endregion // Exists

        #region Reuse

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IPattern Reuse(
                            Expression<Func<IVar, IPattern>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IPattern Reuse(
                            Expression<Func<IVar, Func<IVar, IPattern>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IPattern Reuse(
                            Expression<Func<IVar, Func<IVar, Func<IVar, IPattern>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IPattern Reuse(
                            Expression<Func<IVar, Func<IVar, Func<IVar, Func<IVar, IPattern>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IPattern Reuse(
                            Expression<Func<IVar, Func<IVar, Func<IVar, Func<IVar, Func<IVar, IPattern>>>>>> expression,
                            Action<CypherConfig>? configuration = null)
        {
            var cfg = new CypherConfig();
            configuration?.Invoke(cfg);
            return new ExpressionPattern(expression.Body, cfg);
        }

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static IReuse<T, Fluent> Reuse<T>(this T v) => new Reuse<T, Fluent>(f => f(v));

        /// <summary>
        /// Use for encapsulation of reusable expression.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static IReuse<T, Func<U, R>> Reuse<T, U, R>(this T r, IReuse<U, R> v) => new Reuse<T, Func<U, R>>(f => v.By(f(r)));

        #endregion // Reuse

        #region As

        /// <summary>
        /// Define variable as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("$0")]
        public static T As<T>(this IVar var) => throw new NotImplementedException();

        #endregion // As
    }
}
