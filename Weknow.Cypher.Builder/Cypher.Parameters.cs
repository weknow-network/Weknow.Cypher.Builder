﻿using Weknow.GraphDbCommands.Declarations;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.GraphDbCommands
{
    /// <summary>
    /// Entry point for constructing root level Cypher.
    /// For fluent cypher check <see cref="CypherPhraseExtensions" />
    /// </summary>
    public partial class Cypher
    {
        /// <summary>
        /// Parameters factories
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static ParameterDeclaration Create() => ParameterDeclaration.Default;
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static Deconstructor CreateMulti() => Deconstructor.Default;

            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static ParameterDeclaration<T> Create<T>() => ParameterDeclaration<T>.Default;

            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static (ParameterDeclaration<T1>, ParameterDeclaration<T2>) CreateMulti<T1, T2>() => (ParameterDeclaration<T1>.Default, ParameterDeclaration<T2>.Default);
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static (ParameterDeclaration<T1>, ParameterDeclaration<T2>, ParameterDeclaration<T3>) CreateMulti<T1, T2, T3>() => (ParameterDeclaration<T1>.Default, ParameterDeclaration<T2>.Default, ParameterDeclaration<T3>.Default);
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static (ParameterDeclaration<T1>, ParameterDeclaration<T2>, ParameterDeclaration<T3>, ParameterDeclaration<T4>) CreateMulti<T1, T2, T3, T4>() => (ParameterDeclaration<T1>.Default, ParameterDeclaration<T2>.Default, ParameterDeclaration<T3>.Default, ParameterDeclaration<T4>.Default);
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static (ParameterDeclaration<T1>, ParameterDeclaration<T2>, ParameterDeclaration<T3>, ParameterDeclaration<T4>, ParameterDeclaration<T5>) CreateMulti<T1, T2, T3, T4, T5>() => (ParameterDeclaration<T1>.Default, ParameterDeclaration<T2>.Default, ParameterDeclaration<T3>.Default, ParameterDeclaration<T4>.Default, ParameterDeclaration<T5>.Default);
            /// <summary>
            /// Get parameters declaration.
            /// </summary>
            public static (ParameterDeclaration<T1>, ParameterDeclaration<T2>, ParameterDeclaration<T3>, ParameterDeclaration<T4>, ParameterDeclaration<T5>, ParameterDeclaration<T6>) CreateMulti<T1, T2, T3, T4, T5, T6>() => (ParameterDeclaration<T1>.Default, ParameterDeclaration<T2>.Default, ParameterDeclaration<T3>.Default, ParameterDeclaration<T4>.Default, ParameterDeclaration<T5>.Default, ParameterDeclaration<T6>.Default);

            #region Deconstructor

            /// <summary>
            /// Parameters Deconstruction.
            /// </summary>
            public class Deconstructor
            {
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                public static readonly Deconstructor Default = new Deconstructor();

                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                /// <param name="p6">The p6.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5,
                    out ParameterDeclaration p6)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                    p6 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                /// <param name="p6">The p6.</param>
                /// <param name="p7">The p7.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5,
                    out ParameterDeclaration p6,
                    out ParameterDeclaration p7)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                    p6 = ParameterDeclaration.Default;
                    p7 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                /// <param name="p6">The p6.</param>
                /// <param name="p7">The p7.</param>
                /// <param name="p8">The p8.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5,
                    out ParameterDeclaration p6,
                    out ParameterDeclaration p7,
                    out ParameterDeclaration p8)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                    p6 = ParameterDeclaration.Default;
                    p7 = ParameterDeclaration.Default;
                    p8 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                /// <param name="p6">The p6.</param>
                /// <param name="p7">The p7.</param>
                /// <param name="p8">The p8.</param>
                /// <param name="p9">The p9.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5,
                    out ParameterDeclaration p6,
                    out ParameterDeclaration p7,
                    out ParameterDeclaration p8,
                    out ParameterDeclaration p9)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                    p6 = ParameterDeclaration.Default;
                    p7 = ParameterDeclaration.Default;
                    p8 = ParameterDeclaration.Default;
                    p9 = ParameterDeclaration.Default;
                }
                /// <summary>
                /// Parameters Deconstruction.
                /// </summary>
                /// <param name="p1">The p1.</param>
                /// <param name="p2">The p2.</param>
                /// <param name="p3">The p3.</param>
                /// <param name="p4">The p4.</param>
                /// <param name="p5">The p5.</param>
                /// <param name="p6">The p6.</param>
                /// <param name="p7">The p7.</param>
                /// <param name="p8">The p8.</param>
                /// <param name="p9">The p9.</param>
                /// <param name="p10">The P10.</param>
                public void Deconstruct(
                    out ParameterDeclaration p1,
                    out ParameterDeclaration p2,
                    out ParameterDeclaration p3,
                    out ParameterDeclaration p4,
                    out ParameterDeclaration p5,
                    out ParameterDeclaration p6,
                    out ParameterDeclaration p7,
                    out ParameterDeclaration p8,
                    out ParameterDeclaration p9,
                    out ParameterDeclaration p10)
                {
                    p1 = ParameterDeclaration.Default;
                    p2 = ParameterDeclaration.Default;
                    p3 = ParameterDeclaration.Default;
                    p4 = ParameterDeclaration.Default;
                    p5 = ParameterDeclaration.Default;
                    p6 = ParameterDeclaration.Default;
                    p7 = ParameterDeclaration.Default;
                    p8 = ParameterDeclaration.Default;
                    p9 = ParameterDeclaration.Default;
                    p10 = ParameterDeclaration.Default;
                }
            }

            #endregion // Deconstructor
        }
    }
}
