﻿

// https://neo4j.com/docs/cypher-refcard/current/

using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Common delegate of the Cypher builder
    /// </summary>
    public static class CypherDelegates
    {
        ///// <summary>
        ///// Fluent delegate is the underline used to glue the Cypher expression.
        ///// </summary>
        ///// <param name="var">The variable.</param>
        ///// <returns></returns>
        //public delegate Fluent Fluent(VariableDeclaration var);


        /// <summary>
        /// Fluent Case delegate is the underline used to glue the CASE Cypher expression.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate FluentCase FluentCase(VariableDeclaration var);

        /// <summary>
        /// Fluent Case delegate is the underline used to glue the CASE Cypher expression.
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate FluentCase FluentCaseWhen(VariableDeclaration var);

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
        public delegate ICypherStatement NoVariable();
        /// <summary>
        /// <![CDATA[Pattern delegate of T.
        /// Used for having IVar]]>
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate R Project<R>(VariableDeclaration var);

        public delegate ICypherStatement FluentUnwindAction(VariableDeclaration item);
        public delegate ICypherStatement FluentUnwindAction<T>(VariableDeclaration<T> item);

        public delegate ICypherStatement FluentForEachAction(VariableDeclaration item);
        public delegate ICypherStatement FluentForEachAction<T>(VariableDeclaration<T> item);

        public delegate object FluentToArrayAction(PathRelatedVariableDeclaration item);
        public delegate object FluentToArrayAction<T>(PathRelatedVariableDeclaration<T> item);

    }
}
