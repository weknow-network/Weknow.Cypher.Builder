using System.Linq.Expressions;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Context of the expression
    /// </summary>
    internal class ContextExpression
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextExpression"/> class.
        /// </summary>
        /// <param name="isPluralize">if set to <c>true</c> [is pluralize].</param>
        /// <param name="isSingularize">if set to <c>true</c> [is singularize].</param>
        /// <param name="expression">The expression.</param>
        public ContextExpression(bool isPluralize, bool isSingularize, Expression expression)
        {
            IsPluralize = isPluralize;
            IsSingularize = isSingularize;
            Expression = expression;
        }

        #endregion // Ctor

        #region IsPluralize

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pluralize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is pluralize; otherwise, <c>false</c>.
        /// </value>
        public bool IsPluralize { get; set; }

        #endregion // IsPluralize

        #region IsSingularize

        /// <summary>
        /// Gets or sets a value indicating whether this instance is singularize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is singularize; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingularize { get; set; }

        #endregion // IsSingularize

        #region Expression

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public Expression Expression { get; set; }

        #endregion // Expression
    }
}
