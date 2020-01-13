using System;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Represent contextual label operations.
    /// Enable to add additional common labels like environment or tenants
    /// </summary>
    public interface ICypherContextLabels
    {
        /// <summary>
        /// Adds label from point in the cypher flow.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="additionalLabels">The additional labels.</param>
        /// <returns></returns>
        FluentCypher AddFromHere(string label, params string[] additionalLabels);

        /// <summary>
        /// Removes label from point in the cypher flow.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="additionalLabels">The additional labels.</param>
        /// <returns></returns>
        FluentCypher RemoveFromHere(string label, params string[] additionalLabels);
    }
}
