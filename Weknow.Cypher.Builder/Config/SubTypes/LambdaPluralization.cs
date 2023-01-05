// Pluralize libraries:
// https://github.com/sarathkcm/Pluralize.NET
// https://github.com/rvegajr/Pluralize.NET.Core

namespace Weknow.CypherBuilder;

/// <summary>
/// Pluralization services.
/// </summary>
internal class LambdaPluralization : IPluralization
{
    private readonly Func<string, string> _pluralize;
    private readonly Func<string, string> _singularize;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaPluralization"/> class.
    /// </summary>
    /// <param name="pluralize">The pluralize.</param>
    /// <param name="singularize">The singularize.</param>
    public LambdaPluralization(
        Func<string, string> pluralize,
        Func<string, string> singularize)
    {
        _pluralize = pluralize;
        _singularize = singularize;
    }

    #endregion // Ctor

    #region Pluralize

    /// <summary>
    /// Pluralize a word using the service.
    /// </summary>
    /// <param name="word">The word to pluralize.</param>
    /// <returns>The pluralized word </returns>
    public string Pluralize(string word) => _pluralize(word);

    #endregion // Pluralize

    #region Singularize

    /// <summary>
    /// Singularize a word using the service.
    /// </summary>
    /// <param name="word">The word to singularize.</param>
    /// <returns>The singularized word.</returns>
    public string Singularize(string word) => _singularize(word);

    #endregion // Singularize
}
