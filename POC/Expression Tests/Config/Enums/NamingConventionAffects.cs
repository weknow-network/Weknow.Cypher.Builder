namespace Weknow
{
    /// <summary>
    /// What get affected by the naming convention
    /// </summary>
    public enum NamingConventionAffects
    {
        /// <summary>
        /// disable
        /// </summary>
        None = 0,
        /// <summary>
        /// Node
        /// </summary>
        Label = 1,
        /// <summary>
        /// Relations
        /// </summary>
        Type = Label * 2,
        /// <summary>
        /// All
        /// </summary>
        All = Label | Type
    }
}
