﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Weknow.Cypher.Builder.Declarations;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Entry point for constructing root level Cypher.
    /// For fluent cypher check <see cref="CypherPhraseExtensions" />
    /// </summary>
    partial class Cypher
    {
        /// <summary>
        /// Variables factories
        /// </summary>
        public static class Variables
        {
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static VariableDeclaration Create() => VariableDeclaration.Default;
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static Deconstructor CreateMulti() => Deconstructor.Default;

            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static VariableDeclaration<T> Create<T>() => VariableDeclaration<T>.Default;

            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static (VariableDeclaration<T1>, VariableDeclaration<T2>) CreateMulti<T1, T2>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default);
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>) CreateMulti<T1, T2, T3>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default);
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>) CreateMulti<T1, T2, T3, T4>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default);
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>, VariableDeclaration<T5>) CreateMulti<T1, T2, T3, T4, T5>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default, VariableDeclaration<T5>.Default);
            /// <summary>
            /// Get variables declaration.
            /// </summary>
            public static (VariableDeclaration<T1>, VariableDeclaration<T2>, VariableDeclaration<T3>, VariableDeclaration<T4>, VariableDeclaration<T5>, VariableDeclaration<T6>) CreateMulti<T1, T2, T3, T4, T5, T6>() => (VariableDeclaration<T1>.Default, VariableDeclaration<T2>.Default, VariableDeclaration<T3>.Default, VariableDeclaration<T4>.Default, VariableDeclaration<T5>.Default, VariableDeclaration<T6>.Default);

            #region Deconstructor

            /// <summary>
            /// Variables Deconstruction.
            /// </summary>
            public class Deconstructor
            {
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                public static readonly Deconstructor Default = new Deconstructor();

                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                /// <param name="v6">The v6.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5,
                    out VariableDeclaration v6)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                    v6 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                /// <param name="v6">The v6.</param>
                /// <param name="v7">The v7.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5,
                    out VariableDeclaration v6,
                    out VariableDeclaration v7)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                    v6 = VariableDeclaration.Default;
                    v7 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                /// <param name="v6">The v6.</param>
                /// <param name="v7">The v7.</param>
                /// <param name="v8">The v8.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5,
                    out VariableDeclaration v6,
                    out VariableDeclaration v7,
                    out VariableDeclaration v8)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                    v6 = VariableDeclaration.Default;
                    v7 = VariableDeclaration.Default;
                    v8 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                /// <param name="v6">The v6.</param>
                /// <param name="v7">The v7.</param>
                /// <param name="v8">The v8.</param>
                /// <param name="v9">The v9.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5,
                    out VariableDeclaration v6,
                    out VariableDeclaration v7,
                    out VariableDeclaration v8,
                    out VariableDeclaration v9)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                    v6 = VariableDeclaration.Default;
                    v7 = VariableDeclaration.Default;
                    v8 = VariableDeclaration.Default;
                    v9 = VariableDeclaration.Default;
                }
                /// <summary>
                /// Variables Deconstruction.
                /// </summary>
                /// <param name="v1">The v1.</param>
                /// <param name="v2">The v2.</param>
                /// <param name="v3">The v3.</param>
                /// <param name="v4">The v4.</param>
                /// <param name="v5">The v5.</param>
                /// <param name="v6">The v6.</param>
                /// <param name="v7">The v7.</param>
                /// <param name="v8">The v8.</param>
                /// <param name="v9">The v9.</param>
                /// <param name="v10">The P10.</param>
                public void Deconstruct(
                    out VariableDeclaration v1,
                    out VariableDeclaration v2,
                    out VariableDeclaration v3,
                    out VariableDeclaration v4,
                    out VariableDeclaration v5,
                    out VariableDeclaration v6,
                    out VariableDeclaration v7,
                    out VariableDeclaration v8,
                    out VariableDeclaration v9,
                    out VariableDeclaration v10)
                {
                    v1 = VariableDeclaration.Default;
                    v2 = VariableDeclaration.Default;
                    v3 = VariableDeclaration.Default;
                    v4 = VariableDeclaration.Default;
                    v5 = VariableDeclaration.Default;
                    v6 = VariableDeclaration.Default;
                    v7 = VariableDeclaration.Default;
                    v8 = VariableDeclaration.Default;
                    v9 = VariableDeclaration.Default;
                    v10 = VariableDeclaration.Default;
                }
            }

            #endregion // Deconstructor
        }
    }
}
