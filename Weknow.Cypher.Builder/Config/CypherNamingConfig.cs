using Pluralize.NET;

namespace Weknow
{
    /// <summary>
    /// Naming convention
    /// </summary>
    [DebuggerDisplay("Node: {NodeLabelConvention}, Relation: {RelationTagConvention}")]
    public class CypherNamingConfig
    {
        private readonly IPluralize _pluralizeImp;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherNamingConfig"/> class.
        /// </summary>
        public CypherNamingConfig(CypherConfig parent)
        {
            _pluralizeImp = new Pluralizer();
            Pluralization =
                    new LambdaPluralization(
                                word => _pluralizeImp.Pluralize(word),
                                word => _pluralizeImp.Singularize(word)
                            );
            Parent = parent;
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

        #region FormatByConvention

        /// <summary>
        /// Formats by convention.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="convention">The convention.</param>
        /// <returns></returns>
        public static string FormatByConvention(
                string text,
                CypherNamingConvention convention)
        {
            return convention switch
            {
                CypherNamingConvention.SCREAMING_CASE => text.ToSCREAMING(),
                CypherNamingConvention.camelCase => text.ToCamelCase(),
                CypherNamingConvention.PacalCase => text.ToCamelCase(),
                _ => text
            };
        }

        #endregion // FormatByConvention

        /// <summary>
        /// Gets the parent configuration.
        /// </summary>
        internal CypherConfig Parent { get; }
    }
}
