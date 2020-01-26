using Pluralize.NET;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using static Weknow.Helpers.Helper;

// TODO: discuss with avi whether to have default implementation

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Node: {NodeLabelConvention}, Relation: {RelationTagConvention}")]
    public class CypherNamingConfig : ICypherConvention
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

        #region PropertyParameterConvention

        /// <summary>
        /// Gets or sets the property parameter convention.
        /// </summary>
        public CypherPropertiesConventions PropertyParameterConvention { get; set; } = new CypherPropertiesConventions();

        #endregion // PropertyParameterConvention

        #region RelationTypeConvention

        /// <summary>
        /// Gets or sets the relation type convention.
        /// </summary>
        public CypherNamingConvention RelationTypeConvention { get; set; } = CypherNamingConvention.Default;

        #endregion // RelationTypeConvention

        #region FormatLabel

        /// <summary>
        /// Formats the label.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string FormatLabel(string text) =>
            FormatByConvention(text, NodeLabelConvention);

        #endregion // FormatLabel

        #region FormatRelation

        /// <summary>
        /// Formats the relation.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string FormatRelation(string text) =>
            FormatByConvention(text, RelationTypeConvention);

        #endregion // FormatRelation

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
