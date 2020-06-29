using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using static Weknow.NamingConventionAffects;

namespace Weknow
{
    /// <summary>
    /// Label configuration
    /// </summary>
    public class CypherAmbientLabelConfig 
    {
        private readonly CypherNamingConfig _naming;

        public CypherAmbientLabelConfig(CypherNamingConfig naming)
        {
            _naming = naming;
        }

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
        /// <param name="additionalLabels">The additional labels.</param>
        public void Add(params string[] additionalLabels)
        {
            Values = Values.AddRange(additionalLabels);
        }

        #endregion // Add

        #region Combine

        /// <summary>
        /// Get combined label string with ambient and additional labels.
        /// </summary>
        /// <param name="additionalLabels">The labels.</param>
        /// <returns></returns>
        internal string Combine(params string[] additionalLabels)
        {
            return Combine((IEnumerable<string>)additionalLabels);
        }

        /// <summary>
        /// Get combined label string with ambient and additional labels.
        /// </summary>
        /// <param name="additionalLabels">The labels.</param>
        /// <returns></returns>
        internal string Combine(IEnumerable<string> additionalLabels)
        {
            IEnumerable<string> formatted = additionalLabels.Select(m => FormatByConvention(m));
            var values = Values.Select(m => AmbientFormat(m));
            formatted = formatted.Concat(values);
            string result = string.Join(":", formatted);
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
        private protected string FormatByConvention<T>(T text)
        {
            bool formatLabel = (_naming.ConventionAffects & Label) != None;
            CypherNamingConvention convention = formatLabel ? _naming.Convention : CypherNamingConvention.Default;
            string statement = text?.ToString() ?? throw new ArgumentNullException(nameof(text));
            string result = CypherNamingConfig.FormatByConvention(statement, convention);
            return result;
        }

        #endregion // FormatByConvention

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Combine();

        #endregion // ToString
    }
}
