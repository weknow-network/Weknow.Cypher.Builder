using Weknow.Cypher.Builder.Fluent;

namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Relation's Type primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The KNOWS in the following expression will use the Type primitive.
    /// MATCH (n:Person)-[:KNOWS]->(m:Person)
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IType : ICypherToken, IEnumerable<IType>
    {
        /// <summary>
        /// Creates a mock object's type.
        /// </summary>
        /// <returns></returns>
        public readonly static IType Fake = Stub.Empty;

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n)-[:KNOWS|LOVES]->(m)]]></example>
        public static IType operator |(IType a, IType b) => Stub.Empty;
        /// <summary>
        /// <![CDATA[Implements the operator &.]]>
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n)-[:KNOWS&LOVES]->(m)]]></example>
        public static IType operator &(IType a, IType b) => Stub.Empty;
        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <example><![CDATA[(n)-[:!KNOWS]->(m)]]></example>
        public static IType operator !(IType a) => Stub.Empty;

        /// <summary>
        /// Implements the operator op_Multiply with represent a range.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="range">The range.</param>
        /// <example>
        /// <![CDATA[MATCH (n)-[*..5]->(m)]]>
        /// </example>
        public static IType operator *(IType a, int range) => Stub.Empty;

        /// <summary>
        /// Implements the operator op_Multiply with represent a range.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="range">The range.</param>
        /// <example>
        /// <![CDATA[MATCH (n)-[*..5]->(m)]]>
        /// </example>
        public static IType operator *(IType a, System.Range range) => Stub.Empty;
    }
}
