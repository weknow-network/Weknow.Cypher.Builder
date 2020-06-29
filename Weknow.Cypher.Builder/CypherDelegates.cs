#pragma warning disable CA1063 // Implement IDisposable Correctly

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Common delegate of the Cypher builder
    /// </summary>
    public static class CypherDelegates
    {
        /// <summary>
        /// Pattern delegate
        /// </summary>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate PD PD(IVar var);
        /// <summary>
        /// Pattern delegate for relation definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="var">The variable.</param>
        /// <returns></returns>
        public delegate R PDT<T, R>(IVar<T> var);
        /// <summary>
        /// Pattern delegate expression
        /// </summary>
        /// <returns></returns>
        public delegate PD PDE();
    }
}
