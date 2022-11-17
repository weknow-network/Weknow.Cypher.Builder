using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Cypher Extensions
    /// </summary>
    partial class CypherExtensions
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
                    if (char.ToLower(compare[i]) != char.ToLower(with[i]))
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
        /// <param name="property">The property.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// In(n._.property, items)
        /// result in:
        /// n.property IN [items]
        /// </example>
        [Cypher("$0 IN $1")]
        public static bool In(object property, VariableDeclaration compareWith) => throw new NotImplementedException();

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// In(n._.property, items)
        /// result in:
        /// n.property IN [$items]
        /// </example>
        [Cypher("$0 IN $1")]
        public static bool In(object property, ParameterDeclaration compareWith) => throw new NotImplementedException();

        /// <summary>
        /// IN phrase.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="compareWith">The compare with.</param>
        /// <returns></returns>
        /// <example>
        /// var item = VariablesCreateMulti();
        /// In(n._.property, item.List)
        /// result in:
        /// n.property IN [item.List]
        /// </example>
        [Cypher("$0 IN $1")]
        public static bool In(object property, object compareWith) => throw new NotImplementedException();

        #endregion // In
    }
}
