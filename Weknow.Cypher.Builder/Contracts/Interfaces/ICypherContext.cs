using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Represent contextual operations.
    /// </summary>
    public interface ICypherContext
    {
        /// <summary>
        /// Label Context.
        /// </summary>
        ICypherLabelContext Label { get; }

        /// <summary>
        /// Sets the conventions.
        /// </summary>
        /// <param name="nodeLabelConvention">The node convention.</param>
        /// <param name="relationTagConvention">The relation convention.</param>
        FluentCypher Conventions(
          CypherNamingConvention nodeLabelConvention,
          CypherNamingConvention relationTagConvention);
    }
}
