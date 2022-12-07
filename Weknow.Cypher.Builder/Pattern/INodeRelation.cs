
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

        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INodeRelation operator -(INodeRelation r1, INodeRelation r2) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INodeRelation operator >(INodeRelation r1, INodeRelation r2) => throw new NotImplementedException();
        /// <summary>
        /// Declaration for operator -.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static INodeRelation operator <(INodeRelation r1, INodeRelation r2) => throw new NotImplementedException();
    }

}
