﻿using System.Runtime.CompilerServices;

namespace Weknow.CypherBuilder;

/// <summary>
/// Label configuration
/// </summary>
public class CypherAmbientLabelConfig
{
    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="CypherAmbientLabelConfig" /> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public CypherAmbientLabelConfig(CypherConfig parent)
    {
        Parent = parent;
    }

    #endregion // Ctor

    #region Values

    /// <summary>
    /// Gets the additional ambient labels which will be added to cypher queries
    /// (when the expression is not hard-codded string).
    /// </summary>
    public IImmutableList<string> Values { get; internal set; } = ImmutableList<string>.Empty;

    #endregion // Values

    #region Formatter

    /// <summary>
    /// Gets or sets the formatter label formatter.
    /// For example "`@{0}`"
    /// </summary>
    public string? Formatter { get; set; }

    #endregion // Formatter

    #region Add

    /// <summary>
    /// Adds the additional ambient labels which will be added to cypher queries.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="name">Automatic parameter, should be ignored.</param>
    /// <returns></returns>
    public CypherAmbientLabelConfig Add(
        ILabel label,
        [CallerArgumentExpression(nameof(label))]
        string name = "")
    {
        Values = Values.Add(name);
        return this;
    }

    /// <summary>
    /// Adds the additional ambient labels which will be added to cypher queries.
    /// </summary>
    /// <param name="additionalLabels">The additional labels.</param>
    public void Add(params string[] additionalLabels)
    {
        Values = Values.AddRange(additionalLabels.Except(Values));
    }

    #endregion // Add

    #region Combine

    /// <summary>
    /// Get combined label string with ambient and additional labels.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <param name="additionalLabels">The labels.</param>
    /// <returns></returns>
    internal string Combine(string separator, params string[] additionalLabels)
    {
        if (additionalLabels == null) return string.Empty;
        return Combine(separator, (IEnumerable<string>)additionalLabels);
    }

    /// <summary>
    /// Get combined label string with ambient and additional labels.
    /// </summary>
    /// <param name="separator">The separator.</param>
    /// <param name="additionalLabels">The labels.</param>
    /// <returns></returns>
    internal string Combine(string separator, IEnumerable<string> additionalLabels)
    {
        if (additionalLabels == null) return string.Empty;
        IEnumerable<string> formatted = additionalLabels.Select(m => FormatByConvention(m));

        var values = Enable ? Values.Select(m => AmbientFormat(m)) : ImmutableList<string>.Empty;
        formatted = formatted.Concat(values);

        //var separator = Parent.Separator;
        string result = string.Join(separator, formatted);
        return result;
    }

    #endregion // Combine

    #region AmbientFormat

    /// <summary>
    /// Ambients the format.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    private protected string AmbientFormat(
        string text)
    {
        text = FormatByConvention(text);
        if (Formatter != null)
            return string.Format(Formatter, text);
        return text;
    }

    #endregion // AmbientFormat

    #region FormatByConvention

    /// <summary>
    /// Formats the specified text.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">text</exception>
    internal protected string FormatByConvention<T>(T text)
    {
        var naming = Parent.Naming;
        CypherNamingConvention convention = naming.LabelConvention;
        string statement = text?.ToString() ?? throw new ArgumentNullException(nameof(text));
        string result = CypherNamingConfig.FormatByConvention(statement, convention);
        return result;
    }

    #endregion // FormatByConvention

    #region Parent

    /// <summary>
    /// Gets the parent configuration.
    /// </summary>
    internal CypherConfig Parent { get; }

    #endregion // Parent

    #region Enable

    /// <summary>
    /// Toggle enable state.
    /// Useful for disabling the ambient label from central place.
    /// </summary>
    /// <value>
    ///   <c>true</c> if enable; otherwise, <c>disable</c>.
    /// </value>
    public bool Enable { get; set; } = true;

    #endregion // Enable
}
