using Weknow.Mapping;

namespace Weknow.CypherBuilder
{

    /// <summary>
    /// Cypher Attribute used to specify formatting pattern
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
    public sealed class CypherAttribute : Attribute
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherAttribute"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public CypherAttribute(string format)
        {
            Format = format;
        }

        #endregion // Ctor

        #region Format

        /// <summary>
        /// Gets the format.
        /// </summary>
        public string Format { get; }

        #endregion // Format

        #region Flavor

        /// <summary>
        /// Gets the flavor.
        /// </summary>
        public CypherFlavor Flavor { get; init; } = CypherFlavor.OpenCypher;

        #endregion // Flavor
    }
}
