using System;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Variable primitive for the Cypher expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    /// The n in the following expression will use the variable primitive. 
    /// CREATE (n {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IVar<T> : IVar
    {
        /// <summary>
        /// Gets type representation of the variable.
        /// </summary>
        T _ { get; }
        /// <summary>
        /// Gets type representation of the variable which should be increment.
        /// </summary>
        T Inc { get; }
    }

    /// <summary>
    /// Variable primitive for the Cypher expression.
    /// </summary>
    /// <example>
    /// The n in the following expression will use the variable primitive. 
    /// CREATE (n {name: $value})
    /// </example>
    /// <remarks>
    /// Primitives don't have actual implementation, 
    /// it's a declarative unit which will be evaluate at parsing time (by the visitor). 
    /// </remarks>
    public interface IVar
    {
        /// <summary>
        /// Declaration for operator +.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IVar operator +(IVar l, IVar r) => throw new NotImplementedException();

        /// <summary>
        /// Declaration for unary operator +.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IVar operator +(IVar item) => throw new NotImplementedException();

        /// <summary>
        /// Use mapping technique.
        /// For example: CREATE (n $map)
        /// </summary>
        IMap AsMap { get; }
    }

}
