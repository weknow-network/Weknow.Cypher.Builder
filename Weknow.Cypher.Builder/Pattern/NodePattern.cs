using System.Linq.Expressions;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Represent predefine pattern
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.IPattern" />
    public class NodePattern : ExpressionPattern, INode
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public NodePattern(Expression expression, CypherConfig configuration)
            : base(expression, configuration)
        {
        }

        #endregion // Ctor
    }
}
