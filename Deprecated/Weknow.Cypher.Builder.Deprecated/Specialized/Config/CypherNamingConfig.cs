using Pluralize.NET;

using System;
using System.Diagnostics;

// TODO: discuss with avi whether to have default implementation

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Node: {NodeLabelConvention}, Relation: {RelationTagConvention}")]
    public class CypherNamingConfig
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
