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
        public class C : Lists { }
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

            // TODO: IN, extract, filter, reduce
        }
    }
}
