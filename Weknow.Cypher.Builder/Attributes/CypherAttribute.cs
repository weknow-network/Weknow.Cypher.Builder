
namespace Weknow.GraphDbCommands
{

    /// <summary>
    /// Cypher Attribute used to specify formatting pattern
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
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
    }
}
