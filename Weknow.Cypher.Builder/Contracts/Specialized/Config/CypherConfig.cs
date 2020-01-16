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
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherConfig"/> class.
        /// </summary>
        public CypherConfig()
        {
            Naming = new CypherNamingConfig();
            AmbientLabels = new CypherAmbientLabelConfig(Naming);
        }

        #endregion // Ctor

        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        public CypherAmbientLabelConfig AmbientLabels { get; private set; }

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

        internal CypherConfig Clone(params string[] additionalAmbientLabels)
        {
            return new CypherConfig
            {
                AmbientLabels = AmbientLabels.Clone(additionalAmbientLabels),
                Concurrency = Concurrency,
                Naming = Naming
            };
        }
    }
}
