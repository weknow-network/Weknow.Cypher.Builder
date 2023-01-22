using Weknow.Cypher.Builder.Fluent;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Label primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The Person in the following expression will use the Label primitive.
    /// MATCH (n:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface ILabel : ICypherToken
    {
        /// <summary>
        /// Creates a mock object's label.
        /// </summary>
        /// <returns></returns>
        public readonly static ILabel Fake = Stub.Empty;

        /// <summary>
        /// Use Label as a relation
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public IType R => Stub.Empty;

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n:Person:Animal)]]></example>
        public static ILabel operator &(ILabel a, ILabel b) => Stub.Empty;
        public static ILabel operator |(ILabel a, ILabel b) => Stub.Empty;
        public static ILabel operator !(ILabel a) => Stub.Empty;
    }

}
