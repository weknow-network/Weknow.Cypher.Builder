using System;
using System.Text;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Cypher Extensions
    /// </summary>
    public static class CypherPredicateExtensions
    {
        #region Compare

        /// <summary>
        /// Compares the specified with.
        /// </summary>
        /// <param name="compare">The compare.</param>
        /// <param name="with">The with.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        internal static bool Compare(
            this ReadOnlySpan<char> compare,
            ReadOnlySpan<char> with,
            bool ignoreCase = false)
        {
            if (compare.Length != with.Length)
                return false;
            for (int i = 0; i < compare.Length; i++)
            {
                if (ignoreCase)
                {
                    if (Char.ToLower(compare[i]) != Char.ToLower(with[i]))
                        return false;
                }
                else
                {
                    if (compare[i] != with[i])
                        return false;
                }
            }
            return true;
        }

        #endregion // Compare

        #region In

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="property">The property.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// n.property IN [$value1, $value2]
        /// </example>
        [Cypher("$0\\.$1 IN \\$$2")]
        public static bool In(this IVar variable, IProperty property, IVar compareWith) => throw new NotImplementedException();

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// n.property IN [$value1, $value2]
        /// </example>
        [Cypher("$0 IN \\$$1")]
        public static bool In(this IVar variable, IVar compareWith) => throw new NotImplementedException();

        #endregion // In
    }
}
