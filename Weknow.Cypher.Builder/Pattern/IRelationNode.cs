using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// TODO: [bnaya, 2020-07] test and fix the examples

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Represent relation to node
    /// </summary>
    /// <seealso cref="Weknow.Cypher.Builder.IPattern" />
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
