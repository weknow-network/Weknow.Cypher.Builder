using System;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Reuse contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IReuse<T, U>
    {
        /// <summary>
        /// Pipe the reuse content.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns></returns>
        Fluent By(Func<T, U> a);
    }
}
