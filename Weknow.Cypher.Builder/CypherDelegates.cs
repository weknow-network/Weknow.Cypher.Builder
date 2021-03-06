﻿#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

using Weknow.Cypher.Builder.Declarations;

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Common delegate of the Cypher builder
    /// </summary>
    public static class CypherDelegates
    {
        /// <summary>
        /// Fluent delegate is the underline used to glue the Cypher expression.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate Fluent Fluent(VariableDeclaration var);

        /// <summary>
        /// <![CDATA[Pattern delegate of T.
        /// Used for having IVar<T>]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate R Project<T, R>(VariableDeclaration<T> var);
        /// <summary>
        /// Enable starting point which don't use any variable
        /// </summary>
        /// <returns></returns>
        public delegate Fluent NoVariable();
        /// <summary>
        /// <![CDATA[Pattern delegate of T.
        /// Used for having IVar]]>
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate R Project<R>(VariableDeclaration var);
    }
}
