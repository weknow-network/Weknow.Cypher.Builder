using Pluralize.NET;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

// TODO: discuss with avi whether to have default implementation

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Sign = {Sign}, Prefix = {Prefix}")]
    public class CypherPropertiesConventions : ICypherPropertiesConventions
    {
        /// <summary>
        /// Gets or sets the property parameter's sign.
        /// </summary>
        public string Sign { get; set; } = "$";
        /// <summary>
        /// Gets or sets the property parameter's prefix.
        /// </summary>
        public string Prefix { get; set; } = string.Empty;
    }
}
