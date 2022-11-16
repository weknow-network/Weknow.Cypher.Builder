using System.Linq.Expressions;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Represent relation to node
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.ExpressionPattern" />
    /// <seealso cref="Weknow.CypherBuilder.IRelationNode" />
    public class RelationNodePattern : ExpressionPattern, IRelationNode
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public RelationNodePattern(Expression expression, CypherConfig configuration)
            : base(expression, configuration)
        {
        }

        #endregion // Ctor
    }
}
