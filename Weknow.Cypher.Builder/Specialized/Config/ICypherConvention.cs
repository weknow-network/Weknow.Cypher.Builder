namespace Weknow
{
    /// <summary>
    /// Naming Config
    /// </summary>
    public interface ICypherConvention
    {
        /// <summary>
        /// Formats a label.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string FormatLabel(string text);

        /// <summary>
        /// Formats a relation.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string FormatRelation(string text);
    }
}