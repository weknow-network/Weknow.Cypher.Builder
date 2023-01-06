using Weknow.Mapping;

namespace Weknow.CypherBuilder;

/// <summary>
/// The cypher builder configuration.
/// </summary>
public class CypherConfig
{
    /// <summary>
    /// Using async configuration scope
    /// </summary>
    public static AsyncLocal<Action<CypherConfig>?> Scope = new AsyncLocal<Action<CypherConfig>?>();

    /// <summary>
    /// Initializes a new instance of the <see cref="CypherConfig"/> class.
    /// </summary>
    public CypherConfig()
    {
        AmbientLabels = new CypherAmbientLabelConfig(this);
        Naming = new CypherNamingConfig(this);
        Time = new ConfigTimeConvention();
    }

    /// <summary>
    /// Ambient Label configuration
    /// </summary>
    public CypherAmbientLabelConfig AmbientLabels { get; }

    /// <summary>
    /// Gets the naming convention.
    /// </summary>
    public CypherNamingConfig Naming { get; private set; }

    /// <summary>
    /// The cypher flavor.
    /// </summary>
    public Flavor Flavor { get; set; } = Flavor.OpenCypher;

    /// <summary>
    /// Gets the date/time convention.
    /// </summary>
    public ConfigTimeConvention Time { get; private set; }
}
