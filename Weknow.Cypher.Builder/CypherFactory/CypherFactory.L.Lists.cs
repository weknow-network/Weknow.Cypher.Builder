// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

// TODO: main phrases + prop setup + where

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
    partial class CypherFactory
    {
        /// <summary>
        /// Cypher List Expressions
        /// </summary>
        public class Collections : Lists { }
        /// <summary>
        /// Cypher List Expressions
        /// </summary>
        public class L : Lists { }
        /// <summary>
        /// Cypher List Expressions
        /// </summary>
        public class Lists
        {
            #region Size

            /// <summary>
            /// Number of elements in the list.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            public static FluentCypher Size(string list) => CypherBuilder.Default.Add($"size({list})");

            #endregion // Size

            #region Reverse

            /// <summary>
            /// Reverse the order of the elements in the list.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            public static FluentCypher Reverse(string list) => CypherBuilder.Default.Add($"reverse({list})");

            #endregion // Reverse

            #region Head

            /// <summary>
            /// head() returns the first.
            /// return null for an empty list.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            public static FluentCypher Head(string list) => CypherBuilder.Default.Add($"head({list})");

            #endregion // Head

            #region Last

            /// <summary>
            /// last() the last element of the list.
            /// return null for an empty list.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            public static FluentCypher Last(string list) => CypherBuilder.Default.Add($"last({list})");

            #endregion // Last

            #region Tail

            /// <summary>
            /// tail() returns all but the first element.
            /// return null for an empty list.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            public static FluentCypher Tail(string list) => CypherBuilder.Default.Add($"tail({list})");

            #endregion // Tail

            /// <summary>
            /// Evaluate expression for each element in the list, accumulate the results.
            /// </summary>
            /// <param name="statement">The statement.</param>
            /// <returns></returns>
            /// <example>
            /// reduce(s = "", x IN list | s + x.prop)
            /// </example>
            public static FluentCypher Reduce(string statement) => CypherBuilder.Default.Add($"reduce({statement})");

            /// <summary>
            /// Evaluate expression for each element in the list, accumulate the results.
            /// </summary>
            /// <param name="accumulatorVariable">The variable of the accumulator.</param>
            /// <param name="initValue">The initialize value.</param>
            /// <param name="list">The list.</param>
            /// <param name="item">The item.</param>
            /// <param name="expression">The expression.</param>
            /// <returns></returns>
            /// <example>
            /// Reduce("s", "''", "list", "x", "s + x.prop")
            /// reduce(s = "", x IN list | s + x.prop)
            /// </example>
            public static FluentCypher Reduce(string accumulatorVariable, string initValue, string item,  string list, string expression) =>
                Reduce($"{accumulatorVariable} = {initValue}, {item} IN {list} | {expression}");

            /// <summary>
            /// Evaluate expression for each element in the list, accumulate the results.
            /// </summary>
            /// <param name="contentExpression"></param>
            /// <returns></returns>
            /// <example>
            /// reduce(s = "", x IN list | s + x.prop)
            /// </example>
            public static FluentCypher Reduce(Func<FluentCypher, FluentCypher> contentExpression)
            {
                return CypherBuilder.Default.Composite(contentExpression, CypherPhrase.None, "reduce(", ")");
            }

            /// <summary>
            /// Evaluate expression for each element in the list, accumulate the results.
            /// </summary>
            /// <param name="content">The delegated.</param>
            /// <returns></returns>
            /// <example>
            /// reduce(s = "", x IN list | s + x.prop)
            /// </example>
            public static FluentCypher Reduce(FluentCypher content)
            {
                return CypherBuilder.Default.Composite(content, CypherPhrase.None, "reduce(", ")");
            }


            // TODO:  reduce, [x IN xs WHERE predicate | extraction]
        }
    }
}
