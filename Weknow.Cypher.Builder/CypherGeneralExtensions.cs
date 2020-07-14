using System;
using System.Text;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Extensions
    /// </summary>
    public static class CypherGeneralExtensions
    {
        #region OfType

        /// <summary>
        /// Define variable as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        [Cypher("$0")]
        public static T OfType<T>(this IVar var) => throw new NotImplementedException();

        #endregion // OfType

        #region As

        /// <summary>
        /// Define variable's alias
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [Cypher("$0 AS $1")]
        public static IVar As(this IVar var, object name) => throw new NotImplementedException();

        #endregion // As
    }
}
