using System.Collections.Immutable;

namespace Weknow
{
    /// <summary>
    /// Ambient Label configuration
    /// Use ToString() in order to get formatted labels
    /// </summary>
    public interface ICypherAmbientLabelConfig
    {
        #region Values

        /// <summary>
        /// Gets the additional ambient labels which will be added to cypher queries
        /// (when the expression is not hard-codded string).
        /// </summary>
        IImmutableList<string> Values { get; } 

        #endregion // Values

        #region Formatter

        /// <summary>
        /// Gets or sets the formatter label formatter.
        /// For example "`@{0}`"
        /// </summary>
        string? Formatter { get; }

        #endregion // Formatter
    }
}
