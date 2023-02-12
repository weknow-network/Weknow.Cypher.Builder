using Weknow.Cypher.Builder.Fluent;
using Weknow.CypherBuilder.Declarations;

namespace Weknow.CypherBuilder;

partial interface ICypher
{
    #region FullText

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <returns></returns>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1)\r\n\tYIELD node, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <returns></returns>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1)\r\n\tYIELD node AS $2, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="scoreAlias">Score alias for the YIELD phrase.</param>
    /// <returns></returns>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1)\r\n\tYIELD node AS $2, score AS $3")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        VariableDeclaration scoreAlias) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1, { limit:$2 })\r\n\tYIELD node, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase,
        int limit) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1, { limit:$3 })\r\n\tYIELD node AS $2, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        int limit) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="scoreAlias">Score alias for the YIELD phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("CALL db.index.fulltext.queryNodes('📌$0', $1, { limit:$4 })\r\n\tYIELD node AS $2, score AS $3")]
    [CypherClause]
    public static ICypherStatement FullText(
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        VariableDeclaration scoreAlias,
        int limit) => throw new NotImplementedException();

    #endregion // FullText
}

/// <summary>
/// Cypher Extensions
/// </summary>
public partial class CypherExtensions
{
    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2)\r\n\tYIELD node, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2)\r\n\tYIELD node AS $3, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="scoreAlias">Score alias for the YIELD phrase.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2)\r\n\tYIELD node AS $3, score AS $4")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        VariableDeclaration scoreAlias) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2, { limit:$3 })\r\n\tYIELD node, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase,
        int limit) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2, { limit:$4 })\r\n\tYIELD node AS $3, score")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        int limit) => throw new NotImplementedException();

    /// <summary>
    /// Full Text Search statement.
    /// (requires existence of full text search index)
    /// </summary>
    /// <param name="prev">The previous.</param>
    /// <param name="fullTextIndex">Full index of the text.</param>
    /// <param name="searchPhrase">The search phrase.</param>
    /// <param name="nodeAlias">Node alias for the YIELD phrase.</param>
    /// <param name="scoreAlias">Score alias for the YIELD phrase.</param>
    /// <param name="limit">The limit.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <example>
    /// CALL db.index.fulltext.queryNodes("test_index", 'Healthcare^3 Health*^2 Health~', {limit:50})
    /// </example>
    [Cypher("$0\r\nCALL db.index.fulltext.queryNodes('📌$1', $2, { limit:$5 })\r\n\tYIELD node AS $3, score AS $4")]
    [CypherClause]
    public static ICypherStatement FullText(
        this ICypherStatement prev,
        string fullTextIndex,
        string searchPhrase,
        VariableDeclaration nodeAlias,
        VariableDeclaration scoreAlias,
        int limit) => throw new NotImplementedException();
}
