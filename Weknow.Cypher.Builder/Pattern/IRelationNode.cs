﻿
namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Represent relation to node
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.IPattern" />
    public interface IRelationNode : IPattern
    {
        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator >(INode l, IRelationNode r) => throw new NotImplementedException();
        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator <(INode l, IRelationNode r) => throw new NotImplementedException();

    }

}
