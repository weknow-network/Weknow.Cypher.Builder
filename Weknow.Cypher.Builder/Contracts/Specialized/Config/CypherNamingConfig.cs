using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Node: {NodeLabelConvention}, Relation: {RelationTagConvention}")]
    public class CypherNamingConfig
    {
        #region NodeLabelConvention

        /// <summary>
        /// Gets or sets the node label convention.
        /// </summary>
        public CypherNamingConvention NodeLabelConvention { get; set; } = CypherNamingConvention.Default;

        #endregion // NodeLabelConvention

        #region RelationTagConvention

        /// <summary>
        /// Gets or sets the relation tag convention.
        /// </summary>
        public CypherNamingConvention RelationTagConvention { get; set; } = CypherNamingConvention.Default;

        #endregion // RelationTagConvention
    }
}
