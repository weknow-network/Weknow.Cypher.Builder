
namespace Weknow.CypherBuilder
{
    /// <summary>
    /// Represent alternative to range syntax (currently the range syntax is not supported as expression)
    /// </summary>
    public class Rng
    {
        /// <summary>
        /// Any length
        /// </summary>
        /// <example>
        /// (a)-[*]->(b)
        /// </example>
        public static Rng Any() => throw new NotImplementedException();


        /// <summary>
        /// At least
        /// </summary>
        /// <param name="i">The i.</param>
        /// <example>
        /// (a)-[*3..]->(b)
        /// </example>
        public static Rng AtLeast(int i) => throw new NotImplementedException();

        /// <summary>
        /// At most
        /// </summary>
        /// <param name="i">The i.</param>
        /// <example>
        /// (a)-[*..5]->(b)
        /// </example>
        public static Rng AtMost(int i) => throw new NotImplementedException();

        /// <summary>
        /// From start index to end index.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        /// <example>
        /// (a)-[*3..5]->(b)
        /// </example>
        public static Rng Scope(int start, int end) => throw new NotImplementedException();
    }

}
