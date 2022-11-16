﻿
namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Represent node to relation
    /// </summary>
    /// <seealso cref="Weknow.CypherBuilder.IPattern" />
    public interface INodeRelation : IPattern
    {
        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator >(INodeRelation l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator <(INodeRelation l, INode r) => throw new NotImplementedException();

    }

}
