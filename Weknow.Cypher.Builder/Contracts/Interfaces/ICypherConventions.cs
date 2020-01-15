using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Represent contextual operations.
    /// </summary>
    public interface ICypherConventions
    {
        /// <summary>
        /// When supplied the concurrency field
        /// used for incrementing the concurrency version (Optimistic concurrency)
        /// make sure to set unique constraint (on the matching properties),
        /// otherwise a new node with different concurrency will be created when not match.
        /// </summary>
        /// <param name="nameOfConcurrencyField">The name of concurrency field.</param>
        /// <param name="autoIncrement">if set to <c>true</c> [automatic increment].</param>
        /// <returns></returns>
        FluentCypher Concurrency(string nameOfConcurrencyField, bool autoIncrement = true);

        /// <summary>
        /// Sets the Node naming conventions.
        /// </summary>
        /// <param name="nodeLabelConvention">The node convention.</param>
        FluentCypher NodeNaming(CypherNamingConvention nodeLabelConvention);
        /// <summary>
        /// Sets the Node naming conventions.
        /// </summary>
        /// <param name="relationTagConvention">The relation convention.</param>
        FluentCypher RelationNaming(CypherNamingConvention relationTagConvention);
    }
}
