namespace Weknow
{
    /// <summary>
    /// Represent contextual operations.
    /// </summary>
    public interface IConcurrencyConfig
    {
        /// <summary>
        /// When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency)
        /// make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        string? eTagName { get; }
        /// <summary>
        /// Gets or sets a value indicating whether [automatic increment].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic increment]; otherwise, <c>false</c>.
        /// </value>
        bool AutoIncrement { get; }
    }
}
