// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://github.com/Readify/Neo4jClient/wiki/cypher-examples
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static Weknow.CypherFactory;
using static Weknow.Helpers.Helper;

namespace Weknow
{
    /// <summary>
    /// Fluent Cypher
    /// </summary>
    public static class FluentCypherExtensions 
    {
        #region Scope

        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1>(
            this TParent parent,
            T1 p1,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    FluentCypher /*return*/> scope)
            where TParent : FluentCypher
            where T1 : FluentCypher
        {
            return scope(parent, p1);
        }


        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1, T2>(
            this TParent parent,
            T1 p1,
            T2 p2,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    T2 /*p2*/,
                    FluentCypher /*return*/> scope)
            where TParent : FluentCypher
            where T1 : FluentCypher
            where T2 : FluentCypher
        {
            return scope(parent, p1, p2);
        }


        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1, T2, T3>(
            this TParent parent,
            T1 p1,
            T2 p2,
            T3 p3,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    T2 /*p2*/,
                    T3 /*p3*/,
                    FluentCypher /*return*/> scope)
            where TParent : FluentCypher
            where T1 : FluentCypher
            where T2 : FluentCypher
            where T3 : FluentCypher
        {
            return scope(parent, p1, p2, p3);
        }


        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1, T2, T3, T4>(
            this TParent parent,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    T2 /*p2*/,
                    T3 /*p3*/,
                    T4 /*p4*/,
                    FluentCypher /*return*/> scope)
            where TParent : FluentCypher
            where T1 : FluentCypher
            where T2 : FluentCypher
            where T3 : FluentCypher
            where T4 : FluentCypher
        {
            return scope(parent, p1, p2, p3, p4);
        }


        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1, T2, T3, T4, T5>(
            this TParent parent,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    T2 /*p2*/,
                    T3 /*p3*/,
                    T4 /*p4*/,
                    T5 /*p5*/,
                    FluentCypher /*return*/> scope)
            where TParent : FluentCypher
            where T1 : FluentCypher
            where T2 : FluentCypher
            where T3 : FluentCypher
            where T4 : FluentCypher
            where T5 : FluentCypher
        {
            return scope(parent, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// Scopes enable reusable cypher phrases which can be embed within the cypher query.
        /// Scope can be nested within other scope.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="scope">The scoping expression</param>
        /// <example><![CDATA[ 
        /// .Scope( // parent scope
        ///     P.Create<Foo>(f => f.Id, f => f.Name),
        ///     (parent, p1) =>
        ///         parent.Scope( // nested scope
        ///             N("n1", "LabelA", p1),
        ///             R["r1", "RelB"],
        ///             N("n2", "LabelB", p1),
        ///             R["r2", "RelC"],
        ///             N("n3", "LabelC"),
        ///             1..3),
        ///             (nested, n1, r1, n2, r2, n3, range) => 
        ///             { 
        ///                 return nested.Match(n1 - r1 * range > n2 - r2 > n3);
        ///             })
        ///        )
        ///     ]]></example>
        /// <returns></returns>
        public static FluentCypher Scope<TParent, T1, T2, T3, T4, T5, T6>(
            this TParent parent,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            Func<TParent /*parent*/,
                    T1 /*p1*/,
                    T2 /*p2*/,
                    T3 /*p3*/,
                    T4 /*p4*/,
                    T5 /*p5*/,
                    T6 /*p6*/, 
                    FluentCypher /*return*/> scope)
            where TParent: FluentCypher
            where T1: FluentCypher
            where T2 : FluentCypher
            where T3 : FluentCypher
            where T4 : FluentCypher
            where T5 : FluentCypher
            where T6 : FluentCypher
        {
            return scope(parent, p1, p2, p3, p4, p5, p6);
        }

        #endregion // Scope
    }
}
