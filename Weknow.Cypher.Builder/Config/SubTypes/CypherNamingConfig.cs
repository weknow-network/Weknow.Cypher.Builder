using Pluralize.NET;
using Weknow.CypherBuilder;

namespace Weknow.CypherBuilder;

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

    #region LabelConvention

    /// <summary>
    /// Gets or sets the label's convention.
    /// </summary>
    public CypherNamingConvention LabelConvention { get; set; } = CypherNamingConvention.Default;

    #endregion // LabelConvention

    #region TypeConvention

    /// <summary>
    /// Gets or sets the type's convention.
    /// </summary>
    public CypherNamingConvention TypeConvention { get; set; } = CypherNamingConvention.Default;

    #endregion // TypeConvention

    #region PropertyConvention

    /// <summary>
    /// Gets or sets the property's convention.
    /// </summary>
    public CypherNamingConvention PropertyConvention { get; set; } = CypherNamingConvention.Default;

    #endregion // PropertyConvention

    #region ConvertToTypeConvention

    /// <summary>
    /// Converts to type convention.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    internal string ConvertToTypeConvention(string name)
    {
        string result = FormatByConvention(name, TypeConvention);
        return result;
    }

    #endregion // ConvertToTypeConvention

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
            CypherNamingConvention.PacalCase => text.ToPascalCase(),
            _ => text
        };
    }

    #endregion // FormatByConvention

    #region Parent

    /// <summary>
    /// Gets the parent configuration.
    /// </summary>
    internal CypherConfig Parent { get; }

    #endregion // Parent
}
