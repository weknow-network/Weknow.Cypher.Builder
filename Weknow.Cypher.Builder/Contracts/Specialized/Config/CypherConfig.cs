using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// The cypher builder configuration.
    /// </summary>
    public class CypherConfig
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherConfig"/> class.
        /// </summary>
        public CypherConfig()
        {
            Naming = new CypherNamingConfig();
            Labels = new CypherLabelConfig(Naming);
        }

        #endregion // Ctor

        /// <summary>
        /// Naming related configuration.
        /// </summary>
        public CypherLabelConfig Labels { get; }

        /// <summary>
        /// Gets the naming configuration.
        /// </summary>
        public CypherNamingConfig Naming { get; } = new CypherNamingConfig();

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        public ConcurrencyConfig Concurrency { get; } = new ConcurrencyConfig();
    }
}
