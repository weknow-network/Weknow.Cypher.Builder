using System.Linq.Expressions;

namespace Weknow.GraphDbCommands
{

    /// <summary>
    /// Represent node to relation
    /// </summary>
    /// <seealso cref="Weknow.GraphDbCommands.ExpressionPattern" />
    /// <seealso cref="Weknow.GraphDbCommands.INodeRelation" />
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
