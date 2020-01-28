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
        #region Default

        /// <summary>
        /// The default
        /// </summary>
        public static readonly CypherConfig Default = new CypherConfig();

        #endregion // Default

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

        #region AmbientLabels

        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        public CypherAmbientLabelConfig AmbientLabels { get; private set; }

        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        ICypherAmbientLabelConfig ICypherConfig.AmbientLabels => AmbientLabels;

        #endregion // AmbientLabels

        #region Concurrency

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        public ConcurrencyConfig Concurrency { get; private set; } = new ConcurrencyConfig();

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        
        IConcurrencyConfig ICypherConfig.Concurrency => Concurrency;

        #endregion // Concurrency

        #region Naming

        /// <summary>
        /// Gets the naming convention.
        /// </summary>
        public CypherNamingConfig Naming { get; private set; } = new CypherNamingConfig();

        #endregion // Naming

        #region Convention

        /// <summary>
        /// Gets the convention.
        /// </summary>
        ICypherConvention ICypherConfig.Convention => Naming; 

        #endregion // Convention

        #region Pluralization

        /// <summary>
        /// Gets the pluralization.
        /// </summary>
        IPluralization ICypherConfig.Pluralization => Naming.Pluralization;

        #endregion // Pluralization

        #region Clone

        /// <summary>
        /// Clones the specified additional ambient labels.
        /// </summary>
        /// <param name="additionalAmbientLabels">The additional ambient labels.</param>
        /// <returns></returns>
        internal CypherConfig Clone(params string[] additionalAmbientLabels)
        {
            return new CypherConfig
            {
                AmbientLabels = AmbientLabels.Clone(additionalAmbientLabels),
                Concurrency = Concurrency,
                Naming = Naming
            };
        }

        #endregion // Clone
    }
}
