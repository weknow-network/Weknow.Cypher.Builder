using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Represent contextual label operations.
    /// Enable to add additional common labels like environment or tenants
    /// </summary>
    public interface ICypherLabelContext
    {
        /// <summary>
        /// Gets the current.
        /// </summary>
        IImmutableList<string> Current { get; }

        /// <summary>
        /// Format labels with contextual label.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns></returns>
        string Format(params string[] labels);
        /// <summary>
        /// Format labels with contextual label.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns></returns>
        string Format(IEnumerable<string> labels);

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
