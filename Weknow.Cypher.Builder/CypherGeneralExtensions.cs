using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

using static Weknow.CypherBuilder.CypherDelegates;

namespace Weknow.CypherBuilder
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
        public static VariableDeclaration As(this VariableDeclaration var, string alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="prv"></param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("$0 AS $1")]
        public static ICypherStatement As(this ICypherStatement prv, VariableDeclaration alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="prv"></param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("$0 AS $1")]
        public static ICypherStatement As(this ICypherStatement prv, object alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration As(this object var, VariableDeclaration alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration<T> As<T>(this VariableDeclaration<T> var, string alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration<T> As<T>(this object var, VariableDeclaration<T> alias) => throw new NotImplementedException();

        #endregion // As
    }
}
