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
        /// <param name="prv"></param>
        /// <param name="alias">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        [CypherClause]
        public static ICypherStatement As<T>(this ICypherStatement prv, T alias) => throw new NotImplementedException();

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="alias">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        [CypherClause]
        public static VariableDeclaration As<T>(this object var, T alias) => throw new NotImplementedException();

        #endregion // As
    }
}
