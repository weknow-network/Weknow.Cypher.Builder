using Pluralize.NET;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

// TODO: discuss with avi whether to have default implementation
// TODO: can be extension method but static code analyzer should suggest it when using UNWIND

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Node: {NodeLabelConvention}, Relation: {RelationTagConvention}")]
    public class CypherNamingConfig : ICypherNamingConfig
    {
        private IPluralize _pluralizeImp;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherNamingConfig"/> class.
        /// </summary>
        public CypherNamingConfig()
        {
            _pluralizeImp = new Pluralizer();
            Pluralization =
                    new LambdaPluralization(
                                word => _pluralizeImp.Pluralize(word),
                                word => _pluralizeImp.Singularize(word)
                            );
        }

        #endregion // Ctor

        #region Convention

        /// <summary>
        /// Gets or sets the convention.
        /// </summary>
        public CypherNamingConvention Convention { get; set; } = CypherNamingConvention.Default;

        #endregion // Convention

        #region ConventionAffects

        /// <summary>
        /// Gets or sets the convention affects.
        /// </summary>
        public NamingConventionAffects ConventionAffects { get; set; } = NamingConventionAffects.All;

        #endregion // ConventionAffects

        #region Pluralization

        /// <summary>
        /// Gets or sets the pluralization service.
        /// </summary>
        public IPluralization Pluralization { get; set; }

        #endregion // Pluralization

        #region SetPluralization

        /// <summary>
        /// Sets the pluralization service.
        /// </summary>
        /// <param name="pluralize">The pluralize.</param>
        /// <param name="singularize">The singularize.</param>
        public void SetPluralization(
            Func<string, string> pluralize,
            Func<string, string> singularize)
        {
            Pluralization =
                    new LambdaPluralization(pluralize, singularize);
        }

        #endregion // SetPluralization
    }
}
