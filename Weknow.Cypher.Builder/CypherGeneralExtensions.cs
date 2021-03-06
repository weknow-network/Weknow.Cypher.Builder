﻿using System;
using System.Text;

using Weknow.Cypher.Builder.Declarations;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Extensions
    /// </summary>
    public static class CypherGeneralExtensions
    {
        #region As

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration As(this VariableDeclaration var, object alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration As(this object var, VariableDeclaration alias) => throw new NotImplementedException();

        #endregion // As

        #region AsParameter

        /// <summary>
        /// Project the variable as parameter.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static ParameterDeclaration<T> AsParameter<T>(this VariableDeclaration<T> instance) => throw new NotImplementedException();

        #endregion // AsParameter
    }
}
