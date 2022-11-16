using System.Linq.Expressions;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Represent predefine pattern
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.IPattern" />
    public class ExpressionPattern : IPattern
    {
        internal Expression expression { get; }
        internal CypherConfig configuration { get; }

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionPattern"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public ExpressionPattern(Expression expression, CypherConfig configuration)
        {
            this.expression = expression;
            this.configuration = configuration;
        }

        #endregion // Ctor

        #region Cast Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExpressionPattern"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ExpressionPattern instance)
        {
            return instance.ToString();
        }

        #endregion // Cast Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var visitor = new CypherVisitor(new CypherConfig());
            visitor.Visit(expression);
            string result = visitor.Query.ToString();
            return result;
        }

        #endregion // ToString
    }
}
