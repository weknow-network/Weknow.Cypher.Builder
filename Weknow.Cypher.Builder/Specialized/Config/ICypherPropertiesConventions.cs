namespace Weknow
{
    /// <summary>
    /// Properties Conventions
    /// </summary>
    public interface ICypherPropertiesConventions
    {
        /// <summary>
        /// Gets or sets the property's prefix.
        /// </summary>
        string Prefix { get; set; }
        /// <summary>
        /// Gets or sets the property's sign.
        /// </summary>
        string Sign { get; set; }
    }
}