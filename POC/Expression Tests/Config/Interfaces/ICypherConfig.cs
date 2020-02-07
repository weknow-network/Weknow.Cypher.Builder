namespace Weknow
{
    /// <summary>
    /// The cypher builder configuration.
    /// </summary>
    public interface ICypherConfig
    {
        /// <summary>
        /// Ambient Label configuration.
        /// </summary>
        ICypherAmbientLabelConfig AmbientLabels { get; }

        /// <summary>
        /// Sets the concurrency behavior.
        /// </summary>
        IConcurrencyConfig Concurrency { get; }


        ICypherNamingConfig Naming { get; }
    }
}
