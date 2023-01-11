using System.Runtime.CompilerServices;

namespace Weknow.CypherBuilder;

/// <summary>
/// Label configuration
/// </summary>
public class CypherParamConfig
{
    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="CypherParamConfig" /> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public CypherParamConfig(CypherConfig parent)
    {
        Parent = parent;
    }

    #endregion // Ctor

    #region Convention

    /// <summary>
    /// Gets or sets the property's convention.
    /// </summary>
    public CypherNamingConvention Convention { get; set; } = CypherNamingConvention.Default;

    #endregion // Convention

    #region Convention

    /// <summary>
    /// Gets or sets the convert enum to string.
    /// </summary>
    public bool EnumAsString { get; set; } = true;

    #endregion // Convention

    #region Parent

    /// <summary>
    /// Gets the parent configuration.
    /// </summary>
    internal CypherConfig Parent { get; }

    #endregion // Parent
}
