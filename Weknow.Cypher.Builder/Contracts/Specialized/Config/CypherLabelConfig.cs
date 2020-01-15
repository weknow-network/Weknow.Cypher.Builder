using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Weknow
{
    /// <summary>
    /// Label configuration
    /// </summary>
    public class CypherLabelConfig
    {
        private readonly CypherNamingConfig _namingConfig;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherLabelConfig"/> class.
        /// </summary>
        /// <param name="namingConfig">The naming configuration.</param>
        public CypherLabelConfig(CypherNamingConfig namingConfig)
        {
            _namingConfig = namingConfig;
        }

        #endregion // Ctor

        #region WithLabels

        /// <summary>
        /// Gets the additional labels which will be added to cypher queries.
        /// </summary>
        public IImmutableList<string> WithLabels { get; private set; } = ImmutableList<string>.Empty;

        #endregion // WithLabels

        #region AddLabels

        /// <summary>
        /// Adds the additional labels which will be added to cypher queries.
        /// </summary>
        /// <param name="additionalLabels">The additional labels.</param>
        public void AddLabels(params string[] additionalLabels)
        {
            WithLabels = WithLabels.AddRange(additionalLabels);
        }
        
        #endregion // AddLabels

        #region Format

        /// <summary>
        /// Format labels with contextual label.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns></returns>
        public string Format(params string[] labels)
        {
            return Format((IEnumerable<string>)labels);
        }

        /// <summary>
        /// Format labels with contextual label.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns></returns>
        public string Format(IEnumerable<string> labels)
        {
            IEnumerable<string> formatted = labels.Concat(WithLabels).Select(m => FormatByConvention(m));
            string result = string.Join(":", formatted);
            return result;
        }

        #endregion // Format

        #region FormatByConvention

        /// <summary>
        /// Formats the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="convention">The convention.</param>
        /// <returns></returns>
        private protected string FormatByConvention(
            string text,
            CypherNamingConvention convention = CypherNamingConvention.Default)
        {
            if (convention == CypherNamingConvention.Default)
                convention = _namingConfig.NodeLabelConvention;
            return Helpers.Helper.FormatByConvention(text, convention);
        }

        /// <summary>
        /// Formats the specified text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <param name="convention">The convention.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">text</exception>
        private protected string FormatByConvention<T>(
            T text,
            CypherNamingConvention convention = CypherNamingConvention.Default)
        {
            if (convention == CypherNamingConvention.Default)
                convention = _namingConfig.NodeLabelConvention;
            string statement = text?.ToString() ?? throw new ArgumentNullException(nameof(text));
            return Helpers.Helper.FormatByConvention(statement, convention);
        }

        #endregion // FormatByConvention
    }
}
