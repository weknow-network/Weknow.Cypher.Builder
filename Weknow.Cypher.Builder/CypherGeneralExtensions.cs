using System;
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
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        public static VariableDeclaration As(this VariableDeclaration var, object name) => throw new NotImplementedException();

        #endregion // As
    }
}
