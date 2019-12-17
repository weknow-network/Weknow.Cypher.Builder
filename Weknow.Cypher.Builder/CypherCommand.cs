// https://neo4j.com/docs/cypher-refcard/current/
// https://github.com/Readify/Neo4jClient
// https://neo4jmapper.tk/guide.html
// https://github.com/barnardos-au/Neo4jMapper

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Weknow
{
    /// <summary>
    /// Fluent cypher builder
    /// </summary>
    /// <seealso cref="Weknow.IFluentCypher" />
    [DebuggerDisplay("{CypherLine}")]
    internal class CypherCommand : ICypherable
    {
        public static readonly CypherCommand Empty = new CypherCommand();
        public CypherCommand Previous { get; }
        private static readonly string SEPERATOR = $" {Environment.NewLine}";
        private readonly Regex TrimX = new Regex(@"\s+");
        public CypherPhrase Phrase { get; }

        #region Ctor

        private CypherCommand()
        {
            Cypher = string.Empty;
            Previous = Empty;
        }

        public CypherCommand(
            CypherCommand copyFrom,
            string cypher,
            CypherPhrase phrase)
        {
            Previous = copyFrom;
            Phrase = phrase;
            Cypher = copyFrom.Cypher;
            if (string.IsNullOrEmpty(Cypher))
                Cypher = cypher;
            else if (!string.IsNullOrEmpty(cypher))
                Cypher = $"{Cypher}{SEPERATOR}{cypher}";
        }

        #endregion // Ctor

        #region Cypher

        /// <summary>
        /// Gets the cypher statement.
        /// </summary>
        public string Cypher { get; }

        #endregion // Cypher

        #region CypherLine

        /// <summary>
        /// Gets the cypher statement trimmed into single line.
        /// </summary>
        public string CypherLine => TrimX.Replace(Cypher, " ").Trim();

        #endregion // CypherLine
    }
}