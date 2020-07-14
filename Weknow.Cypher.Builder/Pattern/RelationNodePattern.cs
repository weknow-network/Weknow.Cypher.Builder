using System.Linq.Expressions;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Represent relation to node
    /// </summary>
    /// <seealso cref="Weknow.Cypher.Builder.ExpressionPattern" />
    /// <seealso cref="Weknow.Cypher.Builder.IRelationNode" />
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
