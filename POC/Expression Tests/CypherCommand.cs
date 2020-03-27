using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using static Weknow.Cypher.Builder.Cypher;
#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace Weknow.Cypher.Builder
{
    /// <summary>
    /// Representation of Cypher Query & Parameters
    /// </summary>
    public class CypherCommand
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CypherCommand"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        public CypherCommand(
            string query,
            CypherParameters parameters) // TODO: not necessary dictionary
        {
            Query = query;
            Parameters = parameters;
        }

        #endregion // Ctor

        #region Query

        /// <summary>
        /// Representation of Cypher Query.
        /// </summary>
        public string Query { get; }

        #endregion // Query

        #region Parameters

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public CypherParameters Parameters { get; }

        #endregion // Parameters

        #region Casting Overloads

        /// <summary>
        /// Performs an implicit conversion from <see cref="CypherCommand"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CypherCommand command) => command.Query;

        #endregion // Casting Overloads

        #region ToString

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $@"{Query}
---Parameters---
{string.Join(Environment.NewLine, Parameters)}";
        }

        #endregion // ToString
    }

}
