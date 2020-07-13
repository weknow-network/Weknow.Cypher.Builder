using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Pattern primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The line and arrows in the following expression are patterns operations. 
    /// MATCH (n)-[r]->(m)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface INode: IPattern
    {
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator -(INode l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator -(INode l, IRelation r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &gt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator >(INode l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &gt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator >(INode l, IRelation r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &lt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INode operator <(INode l, INode r) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator &lt;.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IRelation operator <(INode l, IRelation r) => throw new NotImplementedException();
    }

}
