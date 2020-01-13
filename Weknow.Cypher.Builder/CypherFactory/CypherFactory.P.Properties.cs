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
            /// <param name="parameterPrefix">Variable prefix.</param>
            /// <param name="parameterSeparator">The variable prefix separator.</param>
            /// <param name="propNames">The property names.</param>
            /// <returns></returns>
            /// <example>
            /// -----------------------------------------------
            /// P.Create(new ["Name", "Id"])
            /// Results in:
            /// { Name: $Name, Id: $Id}
            /// -----------------------------------------------
            /// P.Create(new ["Name", "Id"], "prefix")
            /// Results in:
            /// { Name: $prefix_Name, Id: $prefix_Id}
            /// -----------------------------------------------
            /// P.Create(new ["Name", "Id"], "prefix", ".")
            /// Results in:
            /// { Name: $prefix.Name, Id: $prefix.Id}
            /// </example>
            public static FluentCypher Create(IEnumerable<string> propNames, string? parameterPrefix = null, string parameterSeparator = "_")
            {
                var phrases = propNames.Select(m => string.IsNullOrEmpty(parameterPrefix) ? $"{m}: ${m}" : $"{m}: ${parameterPrefix}{parameterSeparator}{m}");
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
                                        Create(name.ToYield(moreNames));

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
            public static FluentCypher Create<T>(params Expression<Func<T, dynamic>>[] propExpressions)
            { 
                IEnumerable<(string variable, string Name)> vns = 
                                                    from exp in propExpressions
                                                    select ExtractLambdaExpression(exp);

                var phrases = vns.Select(m => $"{m.Name}: ${m.variable}_{m.Name}");
                string sep = SetSeparatorStrategy(phrases);
                string statement = string.Join(sep, phrases);

                string lineBreak = LineSeparatorStrategy(phrases);
                return CypherBuilder.Default.Add($"{{ {lineBreak}{statement}{lineBreak} }}");
            }

            /// <summary>
            /// Compose properties phrase by convention.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="filter">The filter.</param>
            /// <returns></returns>
            public static FluentCypher CreateByConvention<T>(Func<string, bool> filter) =>
                CreateByConvention<T>(string.Empty, filter);


            /// <summary>
            /// Compose properties phrase by convention.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="variable">The variable.</param>
            /// <param name="filter">The filter.</param>
            /// <returns></returns>
            public static FluentCypher CreateByConvention<T>(string variable, Func<string, bool> filter)
            {
                IEnumerable<string> names = GetProperties<T>();
                IEnumerable<string> propNames =
                                names.Where(name => filter(name));
                FluentCypher properties = Create(propNames, variable);
                return properties;
            }

            /// <summary>
            /// Compose properties phrase by reflection with exclude option.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="excludes">The excludes.</param>
            /// <returns></returns>
            public static FluentCypher CreateAll<T>(params Expression<Func<T, dynamic>>[] excludes) =>
                CreateAll<T>(string.Empty, excludes);

            /// <summary>
            /// Compose properties phrase by reflection with exclude option.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="variable">The variable.</param>
            /// <param name="excludes">The excludes.</param>
            /// <returns></returns>
            public static FluentCypher CreateAll<T>(string variable, params Expression<Func<T, dynamic>>[] excludes)
            {
                IEnumerable<string> avoid = from exclude in excludes
                                            let lambda = ExtractLambdaExpression(exclude)
                                            select lambda.Name;
                var excludeMap = avoid.ToDictionary(m => m);

                FluentCypher properties =
                    CreateByConvention<T>(variable, name => !excludeMap.ContainsKey(name));
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