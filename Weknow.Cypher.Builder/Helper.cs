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

namespace Weknow.Helpers
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.IFluentCypher" />
    [DebuggerDisplay("{CypherLine}")]
    internal static class Helper 
    {
        internal const int BREAK_LINE_ON = 3;
        internal static readonly string LINE_SEPERATOR = $"{Environment.NewLine}    ";
        internal static readonly string SET_SEPERATOR = $"{Environment.NewLine}    ,";

        #region ExtractLambdaExpression

        /// <summary>
        /// Extracts the lambda expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The exclude.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        internal static (string variable, string Name) ExtractLambdaExpression<T>(
            Expression<Func<T, dynamic>> expression)
        {
            if (!(expression is LambdaExpression lambda))
                throw new NotSupportedException();
            if (expression.Body is MemberExpression p)
                return (lambda.Parameters.First().Name, p.Member.Name);
            if (expression.Body is UnaryExpression u)
            {
                if (u.Operand is MemberExpression m)
                {
                    return (lambda.Parameters.First().Name, m.Member.Name);
                }
            }
            throw new NotSupportedException();
        }

        #endregion // ExtractLambdaExpression

        #region ExtractLambdaExpressionParameters

        /// <summary>
        /// Extracts parameters the lambda expression.
        /// </summary>
        /// <param name="exclude">The exclude.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal static string[] ExtractLambdaExpressionParameters(
            Expression exclude)
        {
            if (!(exclude is LambdaExpression lambda))
                throw new NotSupportedException();
            string[] results = lambda.Parameters.Select(p => p.Name).ToArray();
            return results;
        }

        #endregion // ExtractLambdaExpressionParameters

        #region GetProperties

        /// <summary>
        /// Gets the properties names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static IEnumerable<string> GetProperties<T>()
        {
            var pis = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance /*| BindingFlags.DeclaredOnly*/);
            return pis.Select(m => m.Name);
        }

        #endregion // GetProperties

        #region ComposeSetWhere

        /// <summary>
        /// Composes set / where phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        internal static string ComposeSetWhere(
                                string variable,
                                IEnumerable<string> propNames)
        {
            var phrases = propNames.Select(m => $"{variable}.{m} = ${variable}_{m}");
            string sep = SetSeparatorStrategy(propNames);
            string statement = string.Join(sep, phrases);
            return statement;
        }

        #endregion // ComposComposeSetWhereeSet

        #region SeparatorStrategy

        /// <summary>
        /// Separators the strategy.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        internal static string NewLineSeparatorStrategy(IEnumerable<string> propNames)
        {
            string sep = string.Empty;
            if (propNames.Count() >= BREAK_LINE_ON)
                sep = LINE_SEPERATOR;
            return sep;
        } 

        /// <summary>
        /// Separators the strategy.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        internal static string SetSeparatorStrategy(IEnumerable<string> propNames)
        {
            string sep = ", ";
            if (propNames.Count() >= BREAK_LINE_ON)
                sep = SET_SEPERATOR;
            return sep;
        }

        #endregion // SeparatorStrategy

        #region Yilder

        /// <summary>
        /// Yilders the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        internal static IEnumerable<T> Yilder<T>(T item, IEnumerable<T> items)
        {
            yield return item;
            foreach (var element in items)
            {
                yield return element;
            }
        } 

        #endregion // Yilder
    }
}