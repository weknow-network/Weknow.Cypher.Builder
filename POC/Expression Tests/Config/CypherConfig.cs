using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// The cypher builder configuration.
    /// </summary>
    public class CypherConfig: ICypherConfig
    {
        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        public CypherAmbientLabelConfig AmbientLabels { get; } = new CypherAmbientLabelConfig();

        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        ICypherAmbientLabelConfig ICypherConfig.AmbientLabels => AmbientLabels;

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        public ConcurrencyConfig Concurrency { get; private set; } = new ConcurrencyConfig();

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        IConcurrencyConfig ICypherConfig.Concurrency => Concurrency;

        /// <summary>
        /// Gets the naming convention.
        /// </summary>
        public CypherNamingConfig Naming { get; private set; } = new CypherNamingConfig();

        /// <summary>
        /// Gets the pluralization.
        /// </summary>
        ICypherNamingConfig ICypherConfig.Naming => Naming;
    }
}
