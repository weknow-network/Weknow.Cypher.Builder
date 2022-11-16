using System.Linq.Expressions;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Represent node to relation
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.ExpressionPattern" />
    /// <seealso cref="Weknow.CypherBuilder.INodeRelation" />
    public class NodeRelationPattern : ExpressionPattern, INodeRelation
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="configuration">The configuration.</param>
        public NodeRelationPattern(Expression expression, CypherConfig configuration)
            : base(expression, configuration)
        {
        }

        #endregion // Ctor
    }
}
