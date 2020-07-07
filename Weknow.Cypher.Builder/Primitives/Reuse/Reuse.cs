using System;

using static Weknow.Cypher.Builder.CypherDelegates;
#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.Cypher.Builder
{

    /// <summary>
    /// Reuse encapsulation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <seealso cref="Weknow.Cypher.Builder.IReuse{T, U}" />
    internal class Reuse<T, U> : IReuse<T, U>
    {
        private Func<Func<T, U>, Fluent> _by;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Reuse{T, U}"/> class.
        /// </summary>
        /// <param name="by">The by.</param>
        public Reuse(Func<Func<T, U>, Fluent> by)
        {
            _by = by;
        }

        #endregion // Ctor

        #region By

        /// <summary>
        /// Reused by
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns></returns>
        Fluent IReuse<T, U>.By(Func<T, U> a) => _by(a);

        #endregion // By
    }
}
