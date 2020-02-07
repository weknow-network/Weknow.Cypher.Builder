using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Label configuration
    /// </summary>
    public class CypherAmbientLabelConfig : ICypherAmbientLabelConfig
    {
        #region Values

        /// <summary>
        /// Gets the additional ambient labels which will be added to cypher queries
        /// (when the expression is not hard-codded string).
        /// </summary>
        public IImmutableList<string> Values { get; internal set; } = ImmutableList<string>.Empty;

        #endregion // Values

        #region Formatter

        /// <summary>
        /// Gets or sets the formatter label formatter.
        /// For example "`@{0}`"
        /// </summary>
        public string? Formatter { get; set; }

        #endregion // Formatter

        #region Add

        /// <summary>
        /// Adds the additional ambient labels which will be added to cypher queries.
        /// </summary>
        /// <param name="additionalLabels">The additional labels.</param>
        public void Add(params string[] additionalLabels)
        {
            Values = Values.AddRange(additionalLabels);
        }

        #endregion // Add
    }
}
