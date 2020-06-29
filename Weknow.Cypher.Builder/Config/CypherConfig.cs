namespace Weknow
{
    /// <summary>
    /// The cypher builder configuration.
    /// </summary>
    public class CypherConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CypherConfig"/> class.
        /// </summary>
        public CypherConfig()
        {
            AmbientLabels = new CypherAmbientLabelConfig(Naming);

        }

        /// <summary>
        /// Ambient Label configuration
        /// </summary>
        public CypherAmbientLabelConfig AmbientLabels { get; }

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        public ConcurrencyConfig Concurrency { get; private set; } = new ConcurrencyConfig();

        /// <summary>
        /// Gets the naming convention.
        /// </summary>
        public CypherNamingConfig Naming { get; private set; } = new CypherNamingConfig();
    }
}
