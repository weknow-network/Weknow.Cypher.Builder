// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

#pragma warning disable RCS1102 // Make class static.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using static Weknow.Helpers.Helper;
using System.Collections;

namespace Weknow
{
    /// <summary>
    /// Cypher Factories.
    /// </summary>
    partial class CypherFactory
    {
        /// <summary>
        /// Properties factories
        /// </summary>
        public class P : Properties { }
        /// <summary>
        /// Properties factories
        /// </summary>
        public class Properties
        {
            /// <summary>
            /// Compose properties phrase.
            /// </summary>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            /// <example>{ Name: $Name, Id: $Id}</example>
            public static FluentCypher Create(IEnumerable<string> propNames) => CypherBuilder.Default.Add(CreateWithVariable(string.Empty, propNames));

            // TODO: Reduce to FluentCypher chain (make sure that the formatting know to handle properties)

            /// <summary>
            /// Compose properties phrase.
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            /// <example>{ Name: $Name, Id: $Id}</example>
            public static FluentCypher CreateWithVariable(string variable, IEnumerable<string> propNames)
            {
                var phrases = propNames.Select(m => string.IsNullOrEmpty(variable) ? $"{m}: ${m}" : $"{m}: ${variable}_{m}");
                string sep = SetSeparatorStrategy(phrases);
                string statement = string.Join(sep, phrases);

                string lineBreak = LineSeparatorStrategy(phrases);
                return CypherBuilder.Default.Add($"{{ {lineBreak}{statement}{lineBreak} }}");
            }

            /// <summary>
            /// Compose properties phrase.
            /// </summary>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            /// <example>{ Name: $Name, Id: $Id}</example>
            public static FluentCypher Create(string name, params string[] moreNames) =>
                                        CreateWithVariable(string.Empty, name.ToYield(moreNames));

            /// <summary>
            /// Compose properties phrase.
            /// </summary>
            /// <param name="variable">The variable.</param>
            /// <param name="name">The name.</param>
            /// <param name="moreNames">The more names.</param>
            /// <returns></returns>
            /// <example>{ Name: $Name, Id: $Id}</example>
            public static FluentCypher CreateWithVariable(string variable, string name, params string[] moreNames) =>
                                        CreateWithVariable(variable, name.ToYield(moreNames));

            /// <summary>
            /// Compose properties phrase from a type expression.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="propExpressions">The property expressions.</param>
            /// <example>
            /// ComposeProps<Foo>(f => f.Name, f => f.Id)
            /// { Name: $Name, Id: $Id}
            /// </example>
            /// <returns></returns>
            public static FluentCypher Create<T>(params Expression<Func<T, dynamic>>[] propExpressions) =>
                CreateWithVariable<T>(string.Empty, propExpressions);

            /// <summary>
            /// Creates the specified variable.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="variable">The variable.</param>
            /// <param name="propExpressions">The property expressions.</param>
            /// <returns></returns>
            public static FluentCypher CreateWithVariable<T>(string variable, params Expression<Func<T, dynamic>>[] propExpressions)
            {
                IEnumerable<string> phrases = from exp in propExpressions
                                              let vn = ExtractLambdaExpression(exp)
                                              select vn.Name;

                FluentCypher result = CreateWithVariable(variable, phrases);
                return result;
            }

            /// <summary>
            /// Compose properties phrase by convention.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="filter">The filter.</param>
            /// <returns></returns>
            public static FluentCypher CreateByConvention<T>(Func<string, bool> filter) =>
                CreateByConventionWithVariable<T>(string.Empty, filter);


            /// <summary>
            /// Compose properties phrase by convention.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="variable">The variable.</param>
            /// <param name="filter">The filter.</param>
            /// <returns></returns>
            public static FluentCypher CreateByConventionWithVariable<T>(string variable, Func<string, bool> filter)
            {
                IEnumerable<string> names = GetProperties<T>();
                IEnumerable<string> propNames =
                                names.Where(name => filter(name));
                FluentCypher properties = CreateWithVariable(variable, propNames);
                return properties;
            }

            /// <summary>
            /// Compose properties phrase by reflection with exclude option.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="excludes">The excludes.</param>
            /// <returns></returns>
            public static FluentCypher CreateAll<T>(params Expression<Func<T, dynamic>>[] excludes) =>
                CreateAllWithVariable<T>(string.Empty, excludes);

            /// <summary>
            /// Compose properties phrase by reflection with exclude option.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="variable">The variable.</param>
            /// <param name="excludes">The excludes.</param>
            /// <returns></returns>
            public static FluentCypher CreateAllWithVariable<T>(string variable, params Expression<Func<T, dynamic>>[] excludes)
            {
                IEnumerable<string> avoid = from exclude in excludes
                                            let lambda = ExtractLambdaExpression(exclude)
                                            select lambda.Name;
                var excludeMap = avoid.ToDictionary(m => m);

                FluentCypher properties =
                    CreateByConventionWithVariable<T>(variable, name => !excludeMap.ContainsKey(name));
                return properties;
            }

            #region LineSeparatorStrategy

            /// <summary>
            /// Separators the strategy.
            /// </summary>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            private static string LineSeparatorStrategy(IEnumerable<string> propNames)
            {
                string sep = string.Empty;
                if (propNames.Count() >= BREAK_LINE_ON)
                    sep = LINE_INDENT_SEPERATOR;
                return sep;
            }

            #endregion // LineSeparatorStrategy

            #region SetSeparatorStrategy

            /// <summary>
            /// Separators the strategy.
            /// </summary>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            private static string SetSeparatorStrategy(IEnumerable<string> propNames)
            {
                string sep = ", ";
                if (propNames.Count() >= BREAK_LINE_ON)
                    sep = LINE_INDENT_COMMA_SEPERATOR;
                return sep;
            }

            #endregion // SetSeparatorStrategy

        }
    }
}