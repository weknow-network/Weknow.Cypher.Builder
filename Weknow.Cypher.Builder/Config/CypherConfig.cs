namespace Weknow
{
    /// <summary>
    /// The cypher builder configuration.
    /// </summary>
    public class CypherConfig
    {
        /// <summary>
        /// Using asyn configuration scope
        /// </summary>
        public static AsyncLocal<Action<CypherConfig>?> Scope = new AsyncLocal<Action<CypherConfig>?>();

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
        /// Gets the naming convention.
        /// </summary>
        public CypherNamingConfig Naming { get; private set; } = new CypherNamingConfig();
    }
}
