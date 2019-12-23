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
using System.Text;

namespace Weknow.Helpers
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.FluentCypher" />
    [DebuggerDisplay("{CypherLine}")]
    internal static class Helper
    {
        internal const int BREAK_LINE_ON = 3;
        internal const string COMMA = " ,";
        internal const string SPACE = " ";
        internal const string INDENT = "    ";
        internal const string HALF_INDENT = "  ";
        internal const string INDENT_COMMA = INDENT + ",";
        internal static readonly string LINE_SEPERATOR = $" {Environment.NewLine}";
        internal static readonly string LINE_INDENT_SEPERATOR = $" {Environment.NewLine}{INDENT}";
        internal static readonly string LINE_INDENT_COMMA_SEPERATOR = $" {Environment.NewLine}{INDENT_COMMA}";

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

        #region FormatSetWhere

        /// <summary>
        /// Formats SET or WHERE phrases.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        public static IEnumerable<string> FormatSetWhere(this IEnumerable<string> propNames, string variable)
        {
            return propNames.Select(m => $"{variable}.{m} = ${variable}_{m}");
        }

        #endregion // FormatSetWhere

        #region SeparatorStrategy

        /// <summary>
        /// Separators the strategy.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        internal static string NewLineSeparatorStrategy(this IEnumerable<string> propNames)
        {
            string sep = string.Empty;
            if (propNames.Count() >= BREAK_LINE_ON)
                sep = LINE_INDENT_SEPERATOR;
            return sep;
        }

        /// <summary>
        /// Separators the strategy.
        /// </summary>
        /// <param name="propNames">The property names.</param>
        /// <returns></returns>
        internal static string SetSeparatorStrategy(IEnumerable<string> propNames)
        {
            string sep = " , ";
            if (propNames.Count() >= BREAK_LINE_ON)
                sep = LINE_INDENT_COMMA_SEPERATOR;
            return sep;
        }

        #endregion // SeparatorStrategy

        #region FormatStatement

        /// <summary>
        /// Formats the statement.
        /// </summary>
        /// <param name="builder">The sb</param>
        /// <param name="current">The current.</param>
        /// <param name="repeat">The repeat.</param>
        /// <returns></returns>
        public static StringBuilder FormatStatement(this StringBuilder builder, FluentCypher current, int repeat)
        {
            switch (current._phrase)
            {
                case CypherPhrase.None:
                case CypherPhrase.Dynamic:
                case CypherPhrase.Set when repeat != 0:
                case CypherPhrase.Where when repeat != 0:
                    break;
                case CypherPhrase.OnCreate:
                case CypherPhrase.OnMatch:
                case CypherPhrase.OrderByDesc:
                case CypherPhrase.And:
                case CypherPhrase.Or:
                    builder = builder.Append(current._phrase.ToString().ToSCREAMING(' '));
                    break;
                case CypherPhrase.OptionalMatch:
                case CypherPhrase.DetachDelete:
                case CypherPhrase.UnionAll:
                case CypherPhrase.OrderBy:
                case CypherPhrase.ReturnDistinct:
                    builder = builder.Append(current._phrase.ToString().ToSCREAMING(' '))
                                     .Append(SPACE);
                    break;
                    builder = builder.Append(CypherPhrase.OrderBy.ToString().ToSCREAMING(' '));
                    break;
                case CypherPhrase.Count:
                    builder = builder.Append(CypherPhrase.Count.ToString().ToLower());
                    break;
                default:
                    builder = builder.Append(current._phrase.ToString().ToUpper())
                                     .Append(SPACE);
                    break;
            }
            builder = builder.Append(current._cypher);
            return builder;
        }

        #endregion // FormatStatement
    }
}