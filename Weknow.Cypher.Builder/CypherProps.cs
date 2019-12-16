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
using static Weknow.N4J.Cypher;

namespace Weknow.N4J
{
    public static class CypherProps
    {
        /// <summary>
        /// Compose properties phrase.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>{ Name: $Name, Id: $Id}</example>
        public static string Create(IEnumerable<string> propNames)
        {
            var phrases = propNames.Select(m => $"{m}: ${m}");
            string sep = SetSeparatorStrategy(phrases);
            string statement = string.Join(sep, phrases);

            string lineBreak = LineSeparatorStrategy(phrases);
            return $"{{ {lineBreak}{statement}{lineBreak} }}";
        }

        /// <summary>
        /// Compose properties phrase.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        /// <example>{ Name: $Name, Id: $Id}</example>
        public static string Create(params string[] propNames) =>
                                    Create((IEnumerable<string>)propNames);

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
        public static string Create<T>(params Expression<Func<T, dynamic>>[] propExpressions)
        {
            IEnumerable<string> phrases = from exp in propExpressions
                                          let vn = ExtractLambdaExpression(exp)
                                          select vn.Name;

            string result = Create(phrases);
            return result;
        }

        /// <summary>
        /// Compose properties phrase by convention.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static string CreateByConvention<T>(Func<string, bool> filter)
        {
            IEnumerable<string> names = GetProperties<T>();
            IEnumerable<string> propNames =
                            names.Where(name => filter(name));
            string properties = Create(propNames);
            return properties;
        }

        /// <summary>
        /// Compose properties phrase by reflection with exclude option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludes">The excludes.</param>
        /// <returns></returns>
        public static string CreateAll<T>(params Expression<Func<T, dynamic>>[] excludes)
        {
            IEnumerable<string> avoid = from exclude in excludes
                                        let lambda = ExtractLambdaExpression(exclude)
                                        select lambda.Name;
            var excludeMap = avoid.ToDictionary(m => m);

            string properties =
                CreateByConvention<T>(name => !excludeMap.ContainsKey(name));
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
                sep = LINE_SEPERATOR;
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
                sep = SET_SEPERATOR;
            return sep;
        }

        #endregion // SetSeparatorStrategy

    }
}